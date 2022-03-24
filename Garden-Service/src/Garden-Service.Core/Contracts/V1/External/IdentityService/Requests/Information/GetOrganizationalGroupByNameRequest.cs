using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Information;

public class GetOrganizationalGroupByNameRequest {
	[Required] public string GroupName { get; set; }
}