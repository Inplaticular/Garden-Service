using Inplanticular.Garden_Service.Core.Contracts.V1.External.CalculationService.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.CalculationService.Responses;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
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
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class PlantService : IPlantService {
	private readonly GardenContext _gardenContext;
	private readonly GatewayOptions _gatewayOptions;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IIdentityService _identityService;
	private readonly IdentityServiceOptions _identityServiceOptions;

	public PlantService(GardenContext gardenContext, IIdentityService identityService,
		IOptions<IdentityServiceOptions> identityServiceOptions, IOptions<GatewayOptions> gatewayOptions,
		IHttpContextAccessor httpContextAccessor) {
		_gardenContext = gardenContext;
		_identityService = identityService;
		_identityServiceOptions = identityServiceOptions.Value;
		_gatewayOptions = gatewayOptions.Value;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<CreatePlantResponse> CreatePlantAsync(CreatePlantRequest request) {
		try {
			var garden = await _gardenContext.FindAsync<Garden>(request.GardenId);
			if (garden is null)
				return new CreatePlantResponse {
					Errors = new[] {
						CreatePlantResponse.Error.PlantCreationError
					}
				};

			if (!await _identityService.CheckUserHasAnyRole(
				    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], garden.UnitId, new[] {
					    GardenRoles.Owner, GardenRoles.Collaborator
				    }))
				throw new UnauthorizedException();

			var group = await _identityService.CreateOrganizationalGroupAsync(new AddOrganizationalGroupRequest {
				Name = _identityServiceOptions.OrganizationalGroupName
			});

			if (group is null)
				return new CreatePlantResponse {
					Errors = new[] {CreatePlantResponse.Error.MissingOrganizationalGroup}
				};

			var plant = new Plant(request.BotanicalName, request.GardenId);

			var unit = await _identityService.CreateOrganizationalUnitAsync(new AddOrganizationalUnitRequest {
				GroupId = group.Id,
				Name = "Plant_" + plant.Id,
				Type = "Plant"
			});

			if (unit is null)
				return new CreatePlantResponse {
					Errors = new[] {CreatePlantResponse.Error.OrganizationalUnitCreationFailed}
				};

			plant.UnitId = unit.Id;
			_gardenContext.Plants.Add(plant);
			await _gardenContext.SaveChangesAsync();

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
			var plant = await _gardenContext.Plants.Include(p => p.Garden)
				.SingleOrDefaultAsync(p => p.Id == request.PlantId);

			if (plant is null)
				return new DeletePlantResponse {
					Errors = new[] {DeletePlantResponse.Error.PlantDeletionErrorIdNotFound}
				};
			if (!await _identityService.CheckUserHasAnyRole(
				    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], plant.Garden.UnitId,
				    new[] {
					    GardenRoles.Owner, GardenRoles.Collaborator
				    }))
				throw new UnauthorizedException();
			_gardenContext.Plants.Remove(plant);
			await _gardenContext.SaveChangesAsync();
			await _identityService.DeleteOrganizationalUnitAsync(new RemoveOrganizationalUnitRequest {
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

	public async Task<GetPlantDataResponse> GetPlantDataAsync() {
		try {
			var platDataList = await _gardenContext.PlantData.ToListAsync();

			return new GetPlantDataResponse {
				Succeeded = true,
				PlantDataList = platDataList,
				Messages = new[] {GetPlantDataResponse.Message.PlantDataReturnSuccessfully}
			};
		}
		catch (Exception e) {
			if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
				return new GetPlantDataResponse {
					Succeeded = false,
					Errors = new[] {GetPlantDataResponse.Error.PlantDataReturnError}
				};

			throw;
		}
	}

	public async Task<GetYieldCalculationResponse> GetYieldCalculationAsync(GetYieldCalculationRequest request) {
		var plant = await _gardenContext.Plants.FindAsync(request.PlantId);

		if (plant is null)
			return new GetYieldCalculationResponse {
				Succeeded = false,
				Errors = new[] {GetYieldCalculationResponse.Error.GetYieldCalculationError}
			};

		var garden = await _gardenContext.Gardens.FindAsync(plant.GardenId);
		if (garden is null)
			return new GetYieldCalculationResponse {
				Succeeded = false,
				Errors = new[] {GetYieldCalculationResponse.Error.GetYieldCalculationError}
			};
		plant.Garden = garden;
		if (!await _identityService.CheckUserHasAnyRole(
			    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], plant.Garden.UnitId, new[] {
				    GardenRoles.Owner, GardenRoles.Collaborator
			    }))
			throw new UnauthorizedException();
		using var httpClient = new HttpClient();
		var yieldRequest = new YieldCalcRequest {
			PlantCoordinateLatitude = garden.CoordinateLatitude,
			PlantCoordinateLongitude = garden.CoordinateLongitude,
			AvgFruitWeight = 250,
			ActFruitCount = plant.ActFruitCount,
			FertilizerPercentage = 1.0,
			DaysWithoutWater = 0
		};

		var response = await httpClient.SendPostAsync<YieldCalcRequest, YieldCalcResponse>(
			_gatewayOptions.Routes.YieldCalculation, yieldRequest);


		if (response is null)
			return new GetYieldCalculationResponse {
				Succeeded = false,
				Errors = new[] {GetYieldCalculationResponse.Error.GetYieldCalculationError}
			};
		if (response.Succeeded)
			return new GetYieldCalculationResponse {
				Succeeded = true,
				Yield = response.Yield,
				Messages = new[] {GetYieldCalculationResponse.Message.GetYieldCalculationSuccessfully}
			};

		return new GetYieldCalculationResponse {
			Succeeded = false,
			Errors = new[] {GetYieldCalculationResponse.Error.GetYieldCalculationError}
		};
	}

	public async Task<GetGrowthCalculationResponse> GetGrowthCalculationAsync(GetGrowthCalculationRequest request) {
		var plant = await _gardenContext.Plants.FindAsync(request.PlantId);

		if (plant is null)
			return new GetGrowthCalculationResponse {
				Succeeded = false,
				Errors = new[] {GetGrowthCalculationResponse.Error.GetGrowthCalculationError}
			};

		var garden = await _gardenContext.Gardens.FindAsync(plant.GardenId);
		if (garden is null)
			return new GetGrowthCalculationResponse {
				Succeeded = false,
				Errors = new[] {GetGrowthCalculationResponse.Error.GetGrowthCalculationError}
			};
		plant.Garden = garden;
		if (!await _identityService.CheckUserHasAnyRole(
			    _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], plant.Garden.UnitId, new[] {
				    GardenRoles.Owner, GardenRoles.Collaborator
			    }))
			throw new UnauthorizedException();
		using var httpClient = new HttpClient();
		var growthRequest = new GrowthCalcRequest {
			PlantCoordinateLatitude = plant.Garden.CoordinateLatitude,
			PlantCoordinateLongitude = plant.Garden.CoordinateLongitude,
			TimeFromPlanting = plant.TimeFromPlanting,
			RipePercentageYesterday = plant.RipePercentage,
			GrowthPerDay = 1.5,
			FertilizerPercentageToday = 1.0,
			DaysWithoutWater = 0
		};
		var response = await httpClient.SendPostAsync<GrowthCalcRequest, GrowthCalcResponse>(
			_gatewayOptions.Routes.GrowthCalculation, growthRequest);


		if (response is null)
			return new GetGrowthCalculationResponse {
				Succeeded = false,
				Errors = new[] {GetGrowthCalculationResponse.Error.GetGrowthCalculationError}
			};
		if (response.Succeeded)
			return new GetGrowthCalculationResponse {
				Succeeded = true,
				GrowthPercentage = response.GrowthPercentage,
				RipeTime = response.RipeTime,
				Messages = new[] {GetGrowthCalculationResponse.Message.GetGrowthCalculationSuccessfully}
			};

		return new GetGrowthCalculationResponse {
			Succeeded = false,
			Errors = new[] {GetGrowthCalculationResponse.Error.GetGrowthCalculationError}
		};
	}
}