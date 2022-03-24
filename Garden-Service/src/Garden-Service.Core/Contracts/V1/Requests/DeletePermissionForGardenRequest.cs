using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record DeletePermissionForGardenRequest {
	[Required] public string Id { get; set; }
}
