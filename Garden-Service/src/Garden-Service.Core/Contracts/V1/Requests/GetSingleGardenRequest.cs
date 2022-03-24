using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record GetSingleGardenRequest {
	[Required] public string GardenId { get; set; }
}