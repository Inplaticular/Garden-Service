using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record CreateGardenRequest {
	[Required] public string Name { get; set; }

	[Required] public string UserId { get; set; }
}