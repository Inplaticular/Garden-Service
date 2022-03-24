namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record DeletePlantRequest {
	public string PlantId { get; set; }
}