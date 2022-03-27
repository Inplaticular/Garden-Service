using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record GetYieldCalculationRequest {
	[Required]public string PlantId { get; set; }

	[Required]public double FertilizerPercentage { get; set; }
	public int ActFruitCount { get; set; }
	[Required]public int DaysWithoutWater { get; set; }
}