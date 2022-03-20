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
	public async Task<IActionResult> CreateGarden(CreateGardenRequest request) {
		var createGardenResponse = await _gardenService.CreateGardenAsync(request);
		return Ok(createGardenResponse);
	}

	[HttpPut(Name = "update_garden")]
	public async Task<IActionResult> UpdateGarden(EditGardenRequest request) {
		var editGardenResponse = await _gardenService.EditGardenAsync(request);
		return Ok(editGardenResponse);
	}

	[HttpDelete(Name = "delete_garden")]
	public async Task<IActionResult> DeleteGarden(DeleteGardenRequest request) {
		var deleteGardenResponse = await _gardenService.DeleteGardenAsync(request);
		return Ok(deleteGardenResponse);
	}
}