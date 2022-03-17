namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record DeleteGardenRequest {
	public int GardenId { get; set; }
}