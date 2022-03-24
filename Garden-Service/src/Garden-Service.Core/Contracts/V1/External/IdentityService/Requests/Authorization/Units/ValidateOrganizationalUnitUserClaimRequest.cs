using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;

public class ValidateOrganizationalUnitUserClaimRequest {
	[Required] public string UnitId { get; set; }
	[Required] public string UserId { get; set; }
	[Required] public string Type { get; set; }
	[Required] public string Value { get; set; }
}