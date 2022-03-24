﻿using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
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
		var garden = new Garden(request.Name, request.UserId, DateTime.UtcNow);
		try {
			_gardenContext.Gardens.Add(garden);
			await _gardenContext.SaveChangesAsync();
			createGardenResponse = new CreateGardenResponse {
				Succeeded = true,
				GardenId = garden.Id,
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

	public async Task<GetGardenResponse> GetGardenAsync(GetGardenRequest request) {
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