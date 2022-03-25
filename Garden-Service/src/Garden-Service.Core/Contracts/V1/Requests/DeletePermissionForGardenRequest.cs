using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record DeletePermissionForGardenRequest {
	[Required] public string GardenId { get; set; }
	[Required] public string PermissionId { get; set; }
}