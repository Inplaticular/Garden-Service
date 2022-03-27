namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record PostYieldCalculationRequest {
	public double PlantCoordinateLatitude { get; set; }
	public double PlantCoordinateLongitude { get; set; }
	public double AvgFruitWeight { get; set; }
	public int ActFruitCount { get; set; }
	public int DaysWithoutWater { get; set; }
	public double FertilizerPercentage { get; set; }
}