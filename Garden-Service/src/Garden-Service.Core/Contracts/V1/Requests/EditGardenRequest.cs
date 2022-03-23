using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record EditGardenRequest {
	[Required] public string GardenId { get; set; }

	[Required] public string Name { get; set; }
}