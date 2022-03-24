using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;

public class RemoveOrganizationalUnitRequest {
	[Required] public string Id { get; set; }
}