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
	public IActionResult CreatePlant(CreatePlantRequest request) {
		var createPlantResponse = _plantService.CreatePlantAsync(request).GetAwaiter().GetResult();
		return Ok(createPlantResponse);
	}

	[HttpDelete(Name = "delete_plant")]
	public IActionResult DeletePlant(DeletePlantRequest request) {
		var deletePlantResponse = _plantService.DeletePlantAsync(request).GetAwaiter().GetResult();
		return Ok(deletePlantResponse);
	}
}