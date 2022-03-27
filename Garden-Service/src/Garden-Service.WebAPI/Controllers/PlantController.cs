using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Exceptions;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.WebAPI.Extensions;
using Inplanticular.Garden_Service.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Inplanticular.Garden_Service.WebAPI.Controllers;

[ApiController]
[Route("v1/plants")]
public class PlantController : ControllerBase {
	private readonly ILogger<PlantController> _logger;
	private readonly IPlantService _plantService;

	public PlantController(IPlantService plantService, ILogger<PlantController> logger) {
		_plantService = plantService;
		_logger = logger;
	}

	/// <summary>
	///     Creates a new plant, the gardenId specifies the garden, the plant growths in.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Your userId is not matching the authentication token, you're logged in with.
	/// </response>
	[HttpPost]
	[UserAuthorized]
	[ProducesResponseType(typeof(CreatePlantResponse), 200)]
	[ProducesResponseType(401)]
	public async Task<IActionResult> CreatePlant(CreatePlantRequest request) {
		try {
			var createPlantResponse = await _plantService.CreatePlantAsync(request);
			return Ok(createPlantResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(CreatePlant)} threw an exception");
			return this.ErrorResponse<CreatePlantResponse>(e);
		}
	}

	/// <summary>
	///     Deletes the plant matching the passed plantId.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Only owner and collaborators are allowed to delete a plant.
	/// </response>
	[HttpDelete]
	[UserAuthorized]
	[ProducesResponseType(typeof(DeletePlantResponse), 200)]
	[ProducesResponseType(401)]
	public async Task<IActionResult> DeletePlant(DeletePlantRequest request) {
		try {
			var deletePlantResponse = await _plantService.DeletePlantAsync(request);
			return Ok(deletePlantResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(DeletePlant)} threw an exception");
			return this.ErrorResponse<DeletePlantResponse>(e);
		}
	}

	/// <summary>
	///     Returns a choose-from-list of all plants currently available in Inplanticular
	/// </summary>
	[HttpGet]
	[Route("plant_data")]
	[ProducesResponseType(typeof(GetPlantDataResponse), 200)]
	public async Task<IActionResult> GetPlantDataAsync() {
		try {
			var getPlantDataResponse = await _plantService.GetPlantDataAsync();
			return Ok(getPlantDataResponse);
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetPlantDataAsync)} threw an exception");
			return this.ErrorResponse<GetPlantDataResponse>(e);
		}
	}

	/// <summary>
	///     Calls Calculation-Service to calculate the yield of a plant manually.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Only owners, collaborators and visitors are allowed to call for yield-calculation.
	/// </response>
	[HttpPost]
	[Route("yield")]
	[UserAuthorized]
	[ProducesResponseType(typeof(GetYieldCalculationResponse), 200)]
	[ProducesResponseType(401)]
	public async Task<IActionResult> GetYieldAsync(GetYieldCalculationRequest request) {
		try {
			var getYieldResponse = await _plantService.GetYieldCalculationAsync(request);
			return Ok(getYieldResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetYieldAsync)} threw an exception");
			return this.ErrorResponse<GetYieldCalculationResponse>(e);
		}
	}

	/// <summary>
	///     Calls Calculation-Service to calculate the growth of a plant manually.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Only owners, collaborators and visitors are allowed to call for growth-calculation.
	/// </response>
	[HttpPost]
	[Route("growth")]
	[UserAuthorized]
	[ProducesResponseType(typeof(GetGrowthCalculationResponse), 200)]
	[ProducesResponseType(401)]
	public async Task<IActionResult> GetGrowthAsync(GetGrowthCalculationRequest request) {
		try {
			var getGrowthResponse = await _plantService.GetGrowthCalculationAsync(request);
			return Ok(getGrowthResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetGrowthAsync)} threw an exception");
			return this.ErrorResponse<GetGrowthCalculationResponse>(e);
		}
	}
}