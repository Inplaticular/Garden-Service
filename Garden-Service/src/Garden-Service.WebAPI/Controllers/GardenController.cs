using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inplanticular.Garden_Service.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GardenController : ControllerBase {
	private readonly IGardenService _gardenService;
	private readonly ILogger<GardenController> _logger;

	public GardenController(IGardenService gardenService, ILogger<GardenController> logger) {
		_gardenService = gardenService;
		_logger = logger;
	}

	[HttpPost(Name = "create_garden")]
	public IActionResult CreateGarden(CreateGardenRequest request) {
		var createGardenResponse = _gardenService.CreateGardenAsync(request).GetAwaiter().GetResult();
		return Ok(createGardenResponse);
	}

	[HttpPut(Name = "update_garden")]
	public IActionResult UpdateGarden(EditGardenRequest request) {
		var editGardenResponse = _gardenService.EditGardenAsync(request).GetAwaiter().GetResult();
		return Ok(editGardenResponse);
	}

	[HttpDelete(Name = "delete_garden")]
	public IActionResult DeleteGarden(DeleteGardenRequest request) {
		var deleteGardenResponse = _gardenService.DeleteGardenAsync(request).GetAwaiter().GetResult();
		return Ok(deleteGardenResponse);
	}
}