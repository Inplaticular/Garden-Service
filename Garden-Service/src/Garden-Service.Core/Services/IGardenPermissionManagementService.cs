using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

namespace Inplanticular.Garden_Service.Core.Services;

public interface IGardenPermissionManagementService {
	GetAssignableRolesResponse GetAssignableRoles();

	Task<GetAssignedPermissionsForGardenResponse> GetPermissionsForGardenAsync(
		GetAssignedPermissionsForGardenRequest request);

	Task<CreatePermissionForGardenResponse> CreatePermissionForGardenAsync(CreatePermissionForGardenRequest request);
	Task<DeletePermissionForGardenResponse> DeletePermissionForGardenAsync(DeletePermissionForGardenRequest request);
}