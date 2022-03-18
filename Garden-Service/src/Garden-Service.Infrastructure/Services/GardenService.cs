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
		var garden = new Garden(request.Name, DateTime.Now);
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
}