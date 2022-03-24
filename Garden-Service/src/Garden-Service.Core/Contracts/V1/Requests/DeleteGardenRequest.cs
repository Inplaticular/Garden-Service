namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record DeleteGardenRequest {
	public string GardenId { get; set; }
}