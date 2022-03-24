using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.WebAPI.Extensions;
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

	[HttpGet]
	[Route("roles")]
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

	[HttpGet]
	public async Task<IActionResult> GetPermissionsForGardenAsync(
		[FromQuery] GetAssignedPermissionsForGardenRequest request) {
		try {
			var getAssignedPermissionsForGardenResponse =
				await _gardenPermissionManagementService.GetPermissionsForGardenAsync(request);
			return Ok(getAssignedPermissionsForGardenResponse);
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetPermissionsForGardenAsync)} threw an exception");
			return this.ErrorResponse<GetAssignedPermissionsForGardenResponse>(e);
		}
	}

	[HttpPost]
	public async Task<IActionResult> CreatePermissionForGardenAsync(CreatePermissionForGardenRequest request) {
		try {
			var createPermissionForGardenResponse =
				await _gardenPermissionManagementService.CreatePermissionForGardenAsync(request);
			return Ok(createPermissionForGardenResponse);
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(GetPermissionsForGardenAsync)} threw an exception");
			return this.ErrorResponse<CreatePermissionForGardenResponse>(e);
		}
	}
	[HttpDelete]
	public async Task<IActionResult> DeletePermissionForGardenAsync(DeletePermissionForGardenRequest request) {
		try {
			var deletePermissionForGardenResponse =
				await _gardenPermissionManagementService.DeletePermissionForGardenAsync(request);
			return Ok(deletePermissionForGardenResponse);
		}
		catch (Exception e) {
			_logger.LogError(e, $"{nameof(DeletePermissionForGardenAsync)} threw an exception");
			return this.ErrorResponse<DeletePermissionForGardenResponse>(e);
		}
	}
}