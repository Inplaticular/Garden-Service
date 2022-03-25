using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record CreatePermissionForGardenRequest {
	[Required] public string GardenId { get; set; }
	[Required] public string UserId { get; set; }
	[Required] public string Type { get; set; }
	[Required] public string Value { get; set; }
}