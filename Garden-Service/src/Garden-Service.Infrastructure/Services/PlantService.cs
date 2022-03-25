using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Enums;
using Inplanticular.Garden_Service.Core.Exceptions;
using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Options;
using Inplanticular.Garden_Service.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class PlantService : IPlantService {
	private readonly GardenContext _gardenContext;
	private readonly IIdentityService _identityService;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IdentityServiceOptions _identityServiceOptions;

	public PlantService(GardenContext gardenContext, IIdentityService identityService, IOptions<IdentityServiceOptions> identityServiceOptions, IHttpContextAccessor httpContextAccessor) {
		this._gardenContext = gardenContext;
		this._identityService = identityService;
		_httpContextAccessor = httpContextAccessor;
		this._identityServiceOptions = identityServiceOptions.Value;
	}

	public async Task<CreatePlantResponse> CreatePlantAsync(CreatePlantRequest request) {
		try {
			var garden = await _gardenContext.FindAsync<Garden>(request.GardenId);
			if (garden is null) {
				return new CreatePlantResponse {
					Errors = new[] {
						CreatePlantResponse.Error.PlantCreationError
					}
				};
			}

			if (!await _identityService.CheckUserHasAnyRole(
				    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], garden.UnitId, new[] {
					    GardenRoles.Owner,GardenRoles.Collaborator
				    }))
				throw new UnauthorizedException();
			
			var group = await this._identityService.CreateOrganizationalGroupAsync(new AddOrganizationalGroupRequest {
				Name = this._identityServiceOptions.OrganizationalGroupName
			});

			if (group is null) {
				return new CreatePlantResponse {
					Errors = new[] {CreatePlantResponse.Error.MissingOrganizationalGroup}
				};
			}
		
			var plant = new Plant(request.BotanicalName, request.GardenId);
		
			var unit = await this._identityService.CreateOrganizationalUnitAsync(new AddOrganizationalUnitRequest {
				GroupId = group.Id,
				Name = "Plant_"+ plant.Id,
				Type = "Plant"
			});

			if (unit is null) {
				return new CreatePlantResponse {
					Errors = new[] {CreatePlantResponse.Error.OrganizationalUnitCreationFailed}
				};
			}

			plant.UnitId = unit.Id;
			this._gardenContext.Plants.Add(plant);
			await this._gardenContext.SaveChangesAsync();
			
			return new CreatePlantResponse {
				Succeeded = true,
				PlantId = plant.Id,
				Messages = new[] {CreatePlantResponse.Message.PlantCreationSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				return new CreatePlantResponse {
					Succeeded = false,
					Errors = new[] {CreatePlantResponse.Error.PlantCreationError}
				};
			
			throw;
		}
	}

	public async Task<DeletePlantResponse> DeletePlantAsync(DeletePlantRequest request) {
		try {
			var plant = await this._gardenContext.Plants.Include(p => p.Garden).SingleOrDefaultAsync(p => p.Id == request.PlantId);

			if (plant is null) {
				return new DeletePlantResponse {
					Errors = new[] {DeletePlantResponse.Error.PlantDeletionErrorIdNotFound}
				};
			}
			if (!await _identityService.CheckUserHasAnyRole(
				    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], plant.Garden.UnitId, new[] {
				GardenRoles.Owner,GardenRoles.Collaborator
			}))
			throw new UnauthorizedException();
			this._gardenContext.Plants.Remove(plant);
			await this._gardenContext.SaveChangesAsync();
			await this._identityService.DeleteOrganizationalUnitAsync(new RemoveOrganizationalUnitRequest {
				Id = plant.UnitId
			});
			
			return new DeletePlantResponse {
				Succeeded = true,
				Messages = new[] {DeletePlantResponse.Message.PlantDeletionSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				return new DeletePlantResponse {
					Succeeded = false,
					Errors = new[] {DeletePlantResponse.Error.PlantDeletionError}
				};
			
			throw;
		}
	}
}