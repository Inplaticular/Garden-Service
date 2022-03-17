using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Services;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class PlantService : IPlantService {
	private readonly GardenContext _gardenContext;

	public PlantService(GardenContext gardenContext) {
		_gardenContext = gardenContext;
	}

	public async Task<CreatePlantResponse> CreatePlantAsync(CreatePlantRequest request) {
		var plant = new Plant(request.FriendlyName, request.BotanicalName, request.DaysToMature, request.GrowthPerDay,
			request.AvgFruitWeight);
		var createPlantResponse = new CreatePlantResponse {
			Succeeded = true,
			PlantId = plant.PlantId,
			Messages = new[] {CreateGardenResponse.Message.GardenCreationSuccessfully}
		};

		_gardenContext.Plants.Add(plant);
		await _gardenContext.SaveChangesAsync();
		return createPlantResponse;
	}

	public async Task<DeletePlantResponse> DeletePlantAsync(DeletePlantRequest request) {
		var plant = new Plant {
			PlantId = request.PlantId
		};
		_gardenContext.Plants.Attach(plant);
		_gardenContext.Plants.Remove(plant);
		await _gardenContext.SaveChangesAsync();
		var deletePlantResponse = new DeletePlantResponse {
			Succeeded = true,
			Messages = new[] {DeleteGardenResponse.Message.GardenDeletionSuccessfully}
		};
		return deletePlantResponse;
	}
}