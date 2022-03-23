using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record GetGardenRequest {
	[Required] 
	public string GardenId { get; set; }
	
	public string UserId { get; set; }
}