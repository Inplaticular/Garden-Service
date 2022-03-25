using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Exceptions;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.WebAPI.Extensions;
using Inplanticular.Garden_Service.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Inplanticular.Garden_Service.WebAPI.Controllers;

[ApiController]
[Route("v1/gardens")]
public class GardenController : ControllerBase {
	private readonly IGardenService _gardenService;
	private readonly IIdentityService _identityService;
	private readonly ILogger<GardenController> _logger;

	public GardenController(IGardenService gardenService, ILogger<GardenController> logger,
		IIdentityService identityService) {
		_gardenService = gardenService;
		_logger = logger;
		_identityService = identityService;
	}

	[HttpPost]
	[UserAuthorized]
	public async Task<IActionResult> CreateGarden(CreateGardenRequest request) {
		try {
			var createGardenResponse = await _gardenService.CreateGardenAsync(request);
			return Ok(createGardenResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(CreateGarden)} threw an exception");
			return this.ErrorResponse<CreateGardenResponse>(e);
		}
	}

	[HttpPut]
	[UserAuthorized]
	public async Task<IActionResult> UpdateGarden(EditGardenRequest request) {
		try {
			var editGardenResponse = await _gardenService.EditGardenAsync(request);
			return Ok(editGardenResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(UpdateGarden)} threw an exception");
			return this.ErrorResponse<EditGardenResponse>(e);
		}
	}

	[HttpDelete]
	[UserAuthorized]
	public async Task<IActionResult> DeleteGarden(DeleteGardenRequest request) {
		try {
			var deleteGardenResponse = await _gardenService.DeleteGardenAsync(request);
			return Ok(deleteGardenResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(DeleteGarden)} threw an exception");
			return this.ErrorResponse<DeleteGardenResponse>(e);
		}
	}

	[HttpGet]
	[UserAuthorized]
	public async Task<IActionResult> GetSingleGardenAsync([FromQuery] GetSingleGardenRequest request) {
		try {
			var getSingleGardenResponse = await _gardenService.GetSingleGardenAsync(request);
			return Ok(getSingleGardenResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetSingleGardenAsync)} threw an exception");
			return this.ErrorResponse<GetSingleGardenResponse>(e);
		}
	}

	[HttpGet]
	[UserAuthorized]
	[Route("list")]
	public async Task<IActionResult> GetGarden([FromQuery] GetGardenRequest request) {
		try {
			var getGardenResponse = await _gardenService.GetGardenAsync(request);
			return Ok(getGardenResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetGarden)} threw an exception");
			return this.ErrorResponse<GetGardenResponse>(e);
		}
	}
}