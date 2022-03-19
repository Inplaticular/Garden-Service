using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Services;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class GardenService : IGardenService {
	private readonly GardenContext _gardenContext;

	public GardenService(GardenContext gardenContext) {
		_gardenContext = gardenContext;
	}


	public async Task<CreateGardenResponse> CreateGardenAsync(CreateGardenRequest request) {
		var garden = new Garden(request.Name, DateTime.UtcNow);
		_gardenContext.Gardens.Add(garden);
		await _gardenContext.SaveChangesAsync();
		var createGardenResponse = new CreateGardenResponse {
			Succeeded = true,
			GardenId = garden.GardenId,
			Messages = new[] {CreateGardenResponse.Message.GardenCreationSuccessfully}
		};
		return createGardenResponse;
	}

	public async Task<DeleteGardenResponse> DeleteGardenAsync(DeleteGardenRequest request) {
		var garden = new Garden {
			GardenId = request.GardenId
		};
		_gardenContext.Gardens.Attach(garden);
		_gardenContext.Gardens.Remove(garden);
		await _gardenContext.SaveChangesAsync();
		var deleteGardenResponse = new DeleteGardenResponse {
			Succeeded = true,
			Messages = new[] {DeleteGardenResponse.Message.GardenDeletionSuccessfully}
		};
		return deleteGardenResponse;
	}

	public async Task<EditGardenResponse> EditGardenAsync(EditGardenRequest request) {
		EditGardenResponse editGardenResponse;
		var garden = _gardenContext.Gardens.SingleOrDefault(g => g.GardenId == request.GardenId);
		if (garden != null) {
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
				editGardenResponse = new EditGardenResponse {
					Succeeded = true,
					Errors = new[] {EditGardenResponse.Error.GardenAlterationErrorGeneral}
				};
				return editGardenResponse;
			}
		}

		editGardenResponse = new EditGardenResponse {
			Succeeded = false,
			Errors = new[] {EditGardenResponse.Error.GardenAlterationErrorIdNotFound}
		};
		return editGardenResponse;
	}
}