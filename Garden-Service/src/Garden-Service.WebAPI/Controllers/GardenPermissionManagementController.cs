using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Exceptions;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.WebAPI.Extensions;
using Inplanticular.Garden_Service.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Inplanticular.Garden_Service.WebAPI.Controllers;

[ApiController]
[Route("v1/gardens/permissions")]
public class GardenPermissionManagementController : ControllerBase {
	private readonly IGardenPermissionManagementService _gardenPermissionManagementService;
	private readonly ILogger<GardenPermissionManagementController> _logger;

	public GardenPermissionManagementController(IGardenPermissionManagementService gardenPermissionManagementService,
		ILogger<GardenPermissionManagementController> logger) {
		_gardenPermissionManagementService = gardenPermissionManagementService;
		_logger = logger;
	}

	/// <summary>
	///     Get all assignable roles available in Inplanticular
	/// </summary>
	[HttpGet]
	[Route("roles")]
	[ProducesResponseType(typeof(GetAssignableRolesResponse), 200)]
	public IActionResult GetAssignableRoles() {
		try {
			var getAssignableRolesResponse = _gardenPermissionManagementService.GetAssignableRoles();
			return Ok(getAssignableRolesResponse);
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetAssignableRoles)} threw an exception");
			return this.ErrorResponse<GetAssignableRolesResponse>(e);
		}
	}

	/// <summary>
	///     Gets permissions for a certain garden
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Your userId is not matching the authentication token, you're logged in with.
	/// </response>
	[HttpGet]
	[UserAuthorized]
	[ProducesResponseType(typeof(GetAssignedPermissionsForGardenResponse), 200)]
	[ProducesResponseType(401)]
	public async Task<IActionResult> GetPermissionsForGardenAsync(
		[FromQuery] GetAssignedPermissionsForGardenRequest request) {
		try {
			var getAssignedPermissionsForGardenResponse =
				await _gardenPermissionManagementService.GetPermissionsForGardenAsync(request);
			return Ok(getAssignedPermissionsForGardenResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetPermissionsForGardenAsync)} threw an exception");
			return this.ErrorResponse<GetAssignedPermissionsForGardenResponse>(e);
		}
	}

	/// <summary>
	///     Creates permissions for the passed user and garden.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Only the owner is capable of creating new permissions
	/// </response>
	[HttpPost]
	[UserAuthorized]
	[ProducesResponseType(typeof(CreatePermissionForGardenResponse), 200)]
	[ProducesResponseType(401)]
	public async Task<IActionResult> CreatePermissionForGardenAsync(CreatePermissionForGardenRequest request) {
		try {
			var createPermissionForGardenResponse =
				await _gardenPermissionManagementService.CreatePermissionForGardenAsync(request);
			return Ok(createPermissionForGardenResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetPermissionsForGardenAsync)} threw an exception");
			return this.ErrorResponse<CreatePermissionForGardenResponse>(e);
		}
	}

	/// <summary>
	///     Deletes permissions for the passed user and garden.
	/// </summary>
	/// <response code="401">
	///     UNAUTHORIZED: Only the owner is capable of deleting permissions.
	/// </response>
	[HttpDelete]
	[UserAuthorized]
	[ProducesResponseType(typeof(DeletePermissionForGardenResponse), 200)]
	[ProducesResponseType(401)]
	public async Task<IActionResult> DeletePermissionForGardenAsync(DeletePermissionForGardenRequest request) {
		try {
			var deletePermissionForGardenResponse =
				await _gardenPermissionManagementService.DeletePermissionForGardenAsync(request);
			return Ok(deletePermissionForGardenResponse);
		}
		catch (UnauthorizedException) {
			return Unauthorized();
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(DeletePermissionForGardenAsync)} threw an exception");
			return this.ErrorResponse<DeletePermissionForGardenResponse>(e);
		}
	}
}