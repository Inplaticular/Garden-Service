namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record GetGrowthCalculationRequest {
	public string PlantId { get; set; }
}