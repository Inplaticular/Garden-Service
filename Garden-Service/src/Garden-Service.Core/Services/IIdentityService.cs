using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
using Inplanticular.Garden_Service.Core.Enums;
using Inplanticular.Garden_Service.Core.Models.External;

namespace Inplanticular.Garden_Service.Core.Services;

public interface IIdentityService {
	Task<OrganizationalGroup?> CreateOrganizationalGroupAsync(AddOrganizationalGroupRequest request);

	Task<OrganizationalUnit?> CreateOrganizationalUnitAsync(AddOrganizationalUnitRequest request);
	Task DeleteOrganizationalUnitAsync(RemoveOrganizationalUnitRequest request);

	bool CheckUserHasId(string token, string userId);

	Task<bool> CheckUserHasAnyRole(string token, string unitId,
		IEnumerable<GardenRoles> gardenRoles);

	Task<bool> CheckPermissionBelongsToUnit(string permissionId, string unitId);
}