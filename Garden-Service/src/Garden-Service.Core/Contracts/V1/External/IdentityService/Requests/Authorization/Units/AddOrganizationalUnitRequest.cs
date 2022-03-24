using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;

public class AddOrganizationalUnitRequest {
	[Required] public string GroupId { get; set; }
	[Required] public string Name { get; set; }
	[Required] public string Type { get; set; }
}