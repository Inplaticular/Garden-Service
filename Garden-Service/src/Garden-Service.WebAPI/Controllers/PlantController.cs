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

	[HttpPost]
	[UserAuthorized]
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

	[HttpDelete]
	[UserAuthorized]
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
}