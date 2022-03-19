using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inplanticular.Garden_Service.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlantController : ControllerBase {
	private readonly ILogger<PlantController> _logger;
	private readonly IPlantService _plantService;

	public PlantController(IPlantService plantService, ILogger<PlantController> logger) {
		_plantService = plantService;
		_logger = logger;
	}

	[HttpPost(Name = "create_plant")]
	public async Task<IActionResult> CreatePlant(CreatePlantRequest request) {
		var createPlantResponse = await _plantService.CreatePlantAsync(request);
		return Ok(createPlantResponse);
	}

	[HttpDelete(Name = "delete_plant")]
	public async Task<IActionResult> DeletePlant(DeletePlantRequest request) {
		var deletePlantResponse = await _plantService.DeletePlantAsync(request);
		return Ok(deletePlantResponse);
	}
}