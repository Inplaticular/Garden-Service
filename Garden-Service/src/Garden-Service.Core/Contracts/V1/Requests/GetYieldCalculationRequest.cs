namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record GetYieldCalculationRequest {
	public string PlantId { get; set; }
}