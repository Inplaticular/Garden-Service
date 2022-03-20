using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class GardenService : IGardenService {
	private readonly GardenContext _gardenContext;

	public GardenService(GardenContext gardenContext) {
		_gardenContext = gardenContext;
	}


	public async Task<CreateGardenResponse> CreateGardenAsync(CreateGardenRequest request) {
		CreateGardenResponse createGardenResponse;
		var garden = new Garden(request.Name, DateTime.UtcNow);
		try {
			_gardenContext.Gardens.Add(garden);
			await _gardenContext.SaveChangesAsync();
			createGardenResponse = new CreateGardenResponse {
				Succeeded = true,
				GardenId = garden.GardenId,
				Messages = new[] {CreateGardenResponse.Message.GardenCreationSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				createGardenResponse = new CreateGardenResponse {
					Succeeded = false,
					Errors = new[] {CreateGardenResponse.Error.GardenCreationError}
				};
			else
				throw;
		}

		return createGardenResponse;
	}

	public async Task<DeleteGardenResponse> DeleteGardenAsync(DeleteGardenRequest request) {
		DeleteGardenResponse deleteGardenResponse;
		var garden = await _gardenContext.Gardens.FindAsync(request.GardenId);
		if (garden is not null)
			try {
				_gardenContext.Gardens.Attach(garden);
				_gardenContext.Gardens.Remove(garden);
				await _gardenContext.SaveChangesAsync();
				deleteGardenResponse = new DeleteGardenResponse {
					Succeeded = true,
					Messages = new[] {DeleteGardenResponse.Message.GardenDeletionSuccessfully}
				};
			}
			catch (Exception e) {
				if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
					deleteGardenResponse = new DeleteGardenResponse {
						Succeeded = false,
						Errors = new[] {DeleteGardenResponse.Error.GardenDeletionError}
					};
				else
					throw;
			}

		deleteGardenResponse = new DeleteGardenResponse {
			Succeeded = false,
			Errors = new[] {DeleteGardenResponse.Error.GardenDeletionErrorIdNotFound}
		};
		return deleteGardenResponse;
	}

	public async Task<EditGardenResponse> EditGardenAsync(EditGardenRequest request) {
		EditGardenResponse editGardenResponse;
		var garden = await _gardenContext.Gardens.FindAsync(request.GardenId);
		if (garden is not null) {
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
}