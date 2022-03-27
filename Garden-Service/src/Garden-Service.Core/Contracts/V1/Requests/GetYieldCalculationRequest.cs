using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record GetYieldCalculationRequest {
	public string PlantId { get; set; }

	public double FertilizerPercentage { get; set; }
	public int ActFruitCount { get; set; }
	public int DaysWithoutWater { get; set; }
}