using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
using Inplanticular.Garden_Service.Core.Models.External;

namespace Inplanticular.Garden_Service.Core.Services;

public interface IIdentityService {
	Task<OrganizationalGroup?> CreateOrganizationalGroupAsync(AddOrganizationalGroupRequest request);
	Task<OrganizationalUnit?> CreateOrganizationalUnitAsync(AddOrganizationalUnitRequest request);
}