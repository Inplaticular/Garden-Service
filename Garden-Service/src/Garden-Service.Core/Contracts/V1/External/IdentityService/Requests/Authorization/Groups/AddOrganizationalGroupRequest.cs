using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;

public class AddOrganizationalGroupRequest {
	[Required] public string Name { get; set; }
}