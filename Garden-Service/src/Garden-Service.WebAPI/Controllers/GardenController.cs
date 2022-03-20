using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.WebAPI.Extensions;
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
		try {
			var createGardenResponse = await _gardenService.CreateGardenAsync(request);
			return Ok(createGardenResponse);
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(CreateGarden)} threw an exception");
			return this.ErrorResponse<CreateGardenResponse>(e);
		}
	}

	[HttpPut(Name = "update_garden")]
	public async Task<IActionResult> UpdateGarden(EditGardenRequest request) {
		try {
			var editGardenResponse = await _gardenService.EditGardenAsync(request);
			return Ok(editGardenResponse);
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(UpdateGarden)} threw an exception");
			return this.ErrorResponse<EditGardenResponse>(e);
		}
	}

	[HttpDelete(Name = "delete_garden")]
	public async Task<IActionResult> DeleteGarden(DeleteGardenRequest request) {
		try {
			var deleteGardenResponse = await _gardenService.DeleteGardenAsync(request);
			return Ok(deleteGardenResponse);
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(DeleteGarden)} threw an exception");
			return this.ErrorResponse<DeleteGardenResponse>(e);
		}
	}
}