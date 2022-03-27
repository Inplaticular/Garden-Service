using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.CalculationService.Requests;

public record YieldCalcRequest {
	public double PlantCoordinateLatitude { get; set; }
	public double PlantCoordinateLongitude { get; set; }

	[Range(1, double.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
	public double AvgFruitWeight { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
	public double ActFruitCount { get; set; }

	[Required] [Range(0, int.MaxValue)] public int DaysWithoutWater { get; set; }
	[Required] public double FertilizerPercentage { get; set; }
}