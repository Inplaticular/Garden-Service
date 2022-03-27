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

	/// <summary>
	///     Creates a garden and sets the passed user as its owner. New gardens does not contain plants.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: It seems your userId is not matching with your authorization token.
	/// </response>
	[HttpPost]
	[UserAuthorized]
	[ProducesResponseType(typeof(CreateGardenResponse), 200)]
	[ProducesResponseType(401)]
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

	/// <summary>
	///     Edits a garden. At the moment, only the name is editable.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Only owners and collaborators are allowed to edit the garden.
	/// </response>
	[HttpPut]
	[UserAuthorized]
	[ProducesResponseType(typeof(EditGardenResponse), 200)]
	[ProducesResponseType(401)]
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

	/// <summary>
	///     Deletes a garden, thereby checking the logged in user is the owner of the garden.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Since owners are the only ones
	///     capable of deleting their garden, it seems you're not the owner of this garden.
	/// </response>
	[HttpDelete]
	[UserAuthorized]
	[ProducesResponseType(typeof(DeleteGardenResponse), 200)]
	[ProducesResponseType(401)]
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

	/// <summary>
	///     Gets the garden with the provided id.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Only owners, collaborators and visitors are allowed to get a single garden.
	/// </response>
	[HttpGet]
	[UserAuthorized]
	[ProducesResponseType(typeof(GetSingleGardenResponse), 200)]
	[ProducesResponseType(401)]
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

	/// <summary>
	///     Gets all gardens, that belong to the user with the passed id.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Your userId is not matching the authentication token, you're logged in with.
	/// </response>
	[HttpGet]
	[UserAuthorized]
	[Route("list")]
	[ProducesResponseType(typeof(GetGardenResponse), 200)]
	[ProducesResponseType(401)]
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