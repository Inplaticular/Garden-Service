using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Options;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class GardenService : IGardenService {
	private readonly GardenContext _gardenContext;
	private readonly IPlantService _plantService;
	private readonly IIdentityService _identityService;
	private readonly IdentityServiceOptions _identityServiceOptions;

	public GardenService(GardenContext gardenContext, IPlantService plantService, IIdentityService identityService, IOptions<IdentityServiceOptions> identityServiceOptions) {
		this._gardenContext = gardenContext;
		this._plantService = plantService;
		this._identityService = identityService;
		this._identityServiceOptions = identityServiceOptions.Value;
	}

	public async Task<CreateGardenResponse> CreateGardenAsync(CreateGardenRequest request) {
		try {
			var group = await this._identityService.CreateOrganizationalGroupAsync(new AddOrganizationalGroupRequest {
				Name = this._identityServiceOptions.OrganizationalGroupName
			});

			if (group is null) {
				return new CreateGardenResponse {
					Errors = new[] {CreateGardenResponse.Error.MissingOrganizationalGroup}
				};
			}
			
			var garden = new Garden(request.Name, request.UserId, DateTime.UtcNow);
			
			var unit = await this._identityService.CreateOrganizationalUnitAsync(new AddOrganizationalUnitRequest {
				GroupId = group.Id,
				Name = "Garden_"+ garden.Id,
				Type = "Garden"
			});

			if (unit is null) {
				return new CreateGardenResponse {
					Errors = new[] {CreateGardenResponse.Error.OrganizationalUnitCreationFailed}
				};
			}
			
			garden.UnitId = unit.Id;
			this._gardenContext.Gardens.Add(garden);
			await this._gardenContext.SaveChangesAsync();
			
			return new CreateGardenResponse {
				Succeeded = true,
				GardenId = garden.Id,
				Messages = new[] {CreateGardenResponse.Message.GardenCreationSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException) {
				return new CreateGardenResponse {
					Succeeded = false,
					Errors = new[] {CreateGardenResponse.Error.GardenCreationError}
				};
			}

			throw;
		}
	}

	public async Task<DeleteGardenResponse> DeleteGardenAsync(DeleteGardenRequest request) {
		try {
			var garden = await this._gardenContext.Gardens.Include(g => g.Plants).SingleOrDefaultAsync(g => g.Id == request.GardenId);

			if (garden is null) {
				return new DeleteGardenResponse {
					Succeeded = false,
					Errors = new[] {DeleteGardenResponse.Error.GardenDeletionErrorIdNotFound}
				};
			}
			
			var deletePlantTasks = garden.Plants.Select(plant => this._plantService.DeletePlantAsync(new DeletePlantRequest { PlantId = plant.Id })).ToArray();
			await Task.WhenAll(deletePlantTasks);
			
			this._gardenContext.Gardens.Remove(garden);
			await this._gardenContext.SaveChangesAsync();

			await this._identityService.DeleteOrganizationalUnitAsync(new RemoveOrganizationalUnitRequest {
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
		var garden = await this._gardenContext.Gardens.FindAsync(request.GardenId);
		if (garden is not null) {
			garden.Name = request.Name;
			try {
				await this._gardenContext.SaveChangesAsync();
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
		GetGardenResponse getGardenResponse;
		var gardenList = await this._gardenContext.Gardens.Where(garden => garden.UserId == request.UserId)
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
		var garden = await this._gardenContext.Gardens
			.Include(g => g.Plants)
			.ThenInclude(p => p.PlantData)
			.SingleOrDefaultAsync(g => g.Id == request.GardenId);
		GetSingleGardenResponse getSingleGardenResponse;
		if (garden is not null)
			try {
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