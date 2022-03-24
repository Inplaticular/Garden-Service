namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization;

public class AuthorizeGlobalRoleRequest : AuthorizeRequest {
	public string GlobalRole { get; set; }
}