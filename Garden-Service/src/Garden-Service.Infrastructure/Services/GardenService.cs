using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Enums;
using Inplanticular.Garden_Service.Core.Exceptions;
using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Options;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class GardenService : IGardenService {
	private readonly GardenContext _gardenContext;
	private readonly GatewayOptions _gatewayOptions;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IIdentityService _identityService;
	private readonly IdentityServiceOptions _identityServiceOptions;
	private readonly ILogger<GardenService> _logger;
	private readonly IPlantService _plantService;


	public GardenService(GardenContext gardenContext, IPlantService plantService, IIdentityService identityService,
		IOptions<IdentityServiceOptions> identityServiceOptions, IOptions<GatewayOptions> gatewayOptions,
		IHttpContextAccessor httpContextAccessor, ILogger<GardenService> logger) {
		_gardenContext = gardenContext;
		_plantService = plantService;
		_identityService = identityService;
		_httpContextAccessor = httpContextAccessor;
		_logger = logger;
		_identityServiceOptions = identityServiceOptions.Value;
		_gatewayOptions = gatewayOptions.Value;
	}

	public async Task<CreateGardenResponse> CreateGardenAsync(CreateGardenRequest request) {
		try {
			if (!_identityService.CheckUserHasId(
				    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization],
				    request.UserId))
				throw new UnauthorizedException();

			var group = await _identityService.CreateOrganizationalGroupAsync(new AddOrganizationalGroupRequest {
				Name = _identityServiceOptions.OrganizationalGroupName
			});

			if (group is null)
				return new CreateGardenResponse {
					Errors = new[] {CreateGardenResponse.Error.MissingOrganizationalGroup}
				};

			var garden = new Garden(request.Name, request.UserId, request.CoordinateLatitude,
				request.CoordinateLongitude, DateTime.UtcNow);

			var unit = await _identityService.CreateOrganizationalUnitAsync(new AddOrganizationalUnitRequest {
				GroupId = group.Id,
				Name = "Garden_" + garden.Id,
				Type = "Garden"
			});

			if (unit is null)
				return new CreateGardenResponse {
					Errors = new[] {CreateGardenResponse.Error.OrganizationalUnitCreationFailed}
				};

			garden.UnitId = unit.Id;
			var createPermissionForGardenRequest = new AddOrganizationalUnitUserClaimRequest {
				UnitId = garden.UnitId,
				UserId = garden.UserId,
				Type = UserClaimTypes.Role.ToString(),
				Value = GardenRoles.Owner.ToString()
			};
			using var httpClient = new HttpClient();
			var response =
				await httpClient
					.SendPostAsync<AddOrganizationalUnitUserClaimRequest, AddOrganizationalUnitUserClaimResponse>(
						_gatewayOptions.Routes.AuthorizationUnitUserClaim, createPermissionForGardenRequest);
			if (response is null || response.Succeeded is false) {
				if (response is not null)
					return
						new CreateGardenResponse {
							Succeeded = false,
							Errors = new[] {CreateGardenResponse.Error.PermissionOfGardenCreationFailed}
								.Concat(response.Errors)
						};
				return
					new CreateGardenResponse {
						Succeeded = false,
						Errors = new[] {CreateGardenResponse.Error.PermissionOfGardenCreationFailed}
					};
			}

			_gardenContext.Gardens.Add(garden);
			await _gardenContext.SaveChangesAsync();

			return new CreateGardenResponse {
				Succeeded = true,
				GardenId = garden.Id,
				Messages = new[] {CreateGardenResponse.Message.GardenCreationSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				return new CreateGardenResponse {
					Succeeded = false,
					Errors = new[] {CreateGardenResponse.Error.GardenCreationError}
				};

			throw;
		}
	}

	public async Task<DeleteGardenResponse> DeleteGardenAsync(DeleteGardenRequest request) {
		try {
			var garden = await _gardenContext.Gardens.Include(g => g.Plants)
				.SingleOrDefaultAsync(g => g.Id == request.GardenId);

			if (garden is null)
				return new DeleteGardenResponse {
					Succeeded = false,
					Errors = new[] {DeleteGardenResponse.Error.GardenDeletionErrorIdNotFound}
				};
			if (!await _identityService.CheckUserHasAnyRole(
				    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], garden.UnitId, new[] {
					    GardenRoles.Owner
				    }))
				throw new UnauthorizedException();

			// Is slow but Database context is not thread-save
			foreach (var plant in garden.Plants)
				await _plantService.DeletePlantAsync(new DeletePlantRequest {PlantId = plant.Id});

			_gardenContext.Gardens.Remove(garden);
			await _gardenContext.SaveChangesAsync();

			await _identityService.DeleteOrganizationalUnitAsync(new RemoveOrganizationalUnitRequest {
				Id = garden.UnitId
			});

			return new DeleteGardenResponse {
				Succeeded = true,
				Messages = new[] {DeleteGardenResponse.Message.GardenDeletionSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				return new DeleteGardenResponse {
					Succeeded = false,
					Errors = new[] {DeleteGardenResponse.Error.GardenDeletionError}
				};

			throw;
		}
	}

	public async Task<EditGardenResponse> EditGardenAsync(EditGardenRequest request) {
		EditGardenResponse editGardenResponse;
		var garden = await _gardenContext.Gardens.FindAsync(request.GardenId);
		if (garden is not null) {
			if (!await _identityService.CheckUserHasAnyRole(
				    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], garden.UnitId, new[] {
					    GardenRoles.Owner, GardenRoles.Collaborator
				    }))
				throw new UnauthorizedException();
			garden.Name = request.Name;
			try {
				await _gardenContext.SaveChangesAsync();
				editGardenResponse = new EditGardenResponse {
					Succeeded = true,
					Messages = new[] {EditGardenResponse.Message.GardenAlterationSuccessfully}
				};
				return editGardenResponse;
			}
			catch (Exception e) {
				if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
					editGardenResponse = new EditGardenResponse {
						Succeeded = false,
						Errors = new[] {EditGardenResponse.Error.GardenAlterationErrorGeneral}
					};
				else
					throw;
			}
		}

		editGardenResponse = new EditGardenResponse {
			Succeeded = false,
			Errors = new[] {EditGardenResponse.Error.GardenAlterationErrorIdNotFound}
		};
		return editGardenResponse;
	}

	public async Task<GetGardenResponse> GetGardenAsync(GetGardenRequest request) {
		if (!_identityService.CheckUserHasId(
			    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization],
			    request.UserId))
			throw new UnauthorizedException();

		GetGardenResponse getGardenResponse;

		var gardenList = await _gardenContext.Gardens.Where(garden => garden.UserId == request.UserId)
			.Include(garden => garden.Plants).ToListAsync();
		try {
			getGardenResponse = new GetGardenResponse {
				Succeeded = true,
				Gardens = gardenList,
				Messages = new[] {GetGardenResponse.Message.GardenReturnSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				getGardenResponse = new GetGardenResponse {
					Succeeded = false,
					Errors = new[] {GetGardenResponse.Error.GardenReturnError}
				};
			else
				throw;
		}

		return getGardenResponse;
	}

	public async Task<GetSingleGardenResponse> GetSingleGardenAsync(GetSingleGardenRequest request) {
		var garden = await _gardenContext.Gardens
			.Include(g => g.Plants)
			.ThenInclude(p => p.PlantData)
			.SingleOrDefaultAsync(g => g.Id == request.GardenId);
		GetSingleGardenResponse getSingleGardenResponse;
		if (garden is not null)
			try {
				if (!await _identityService.CheckUserHasAnyRole(
					    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], garden.UnitId,
					    new[] {
						    GardenRoles.Owner, GardenRoles.Collaborator, GardenRoles.Visitor
					    }))
					throw new UnauthorizedException();
				
				/*// remove circular reference garden -> plant -> garden -> ...
				garden.Plants = garden.Plants.Select(plant => {
					plant.Garden = null!;
					return plant;
				}).ToList();*/
				
				getSingleGardenResponse = new GetSingleGardenResponse {
					Garden = garden,
					Succeeded = true,
					Messages = new[] {GetSingleGardenResponse.Message.SingleGardenReturnSuccessfully}
				};
				return getSingleGardenResponse;
			}
			catch (Exception e) {
				if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
					getSingleGardenResponse = new GetSingleGardenResponse {
						Succeeded = false,
						Errors = new[] {GetSingleGardenResponse.Error.SingleGardenReturnError}
					};
				else
					throw;
			}

		getSingleGardenResponse = new GetSingleGardenResponse {
			Succeeded = false,
			Errors = new[] {GetSingleGardenResponse.Error.SingleGardenReturnErrorIdNotFound}
		};
		return getSingleGardenResponse;
	}
}