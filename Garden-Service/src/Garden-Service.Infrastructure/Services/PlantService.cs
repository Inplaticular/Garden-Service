using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class PlantService : IPlantService {
	private readonly GardenContext _gardenContext;

	public PlantService(GardenContext gardenContext) {
		_gardenContext = gardenContext;
	}

	public async Task<CreatePlantResponse> CreatePlantAsync(CreatePlantRequest request) {
		CreatePlantResponse createPlantResponse;
		var plant = new Plant(request.FriendlyName, request.BotanicalName, request.DaysToMature, request.GrowthPerDay,
			request.AvgFruitWeight);

		try {
			_gardenContext.Plants.Add(plant);
			await _gardenContext.SaveChangesAsync();
			createPlantResponse = new CreatePlantResponse {
				Succeeded = true,
				PlantId = plant.PlantId,
				Messages = new[] {CreatePlantResponse.Message.PlantCreationSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				createPlantResponse = new CreatePlantResponse {
					Succeeded = false,
					Errors = new[] {CreatePlantResponse.Error.PlantCreationError}
				};
			else
				throw;
		}

		return createPlantResponse;
	}

	public async Task<DeletePlantResponse> DeletePlantAsync(DeletePlantRequest request) {
		DeletePlantResponse deletePlantResponse;
		var plant = new Plant {
			PlantId = request.PlantId
		};
		try {
			_gardenContext.Plants.Attach(plant);
			_gardenContext.Plants.Remove(plant);
			await _gardenContext.SaveChangesAsync();
			deletePlantResponse = new DeletePlantResponse {
				Succeeded = true,
				Messages = new[] {DeletePlantResponse.Message.PlantDeletionSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				deletePlantResponse = new DeletePlantResponse {
					Succeeded = false,
					Errors = new[] {DeletePlantResponse.Error.PlantDeletionError}
				};
			else
				throw;
		}

		return deletePlantResponse;
	}
}