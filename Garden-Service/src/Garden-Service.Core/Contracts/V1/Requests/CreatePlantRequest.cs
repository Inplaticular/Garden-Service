namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record CreatePlantRequest {
	public string FriendlyName { get; set; }
	public string BotanicalName { get; set; }
	public int DaysToMature { get; set; }
	public double GrowthPerDay { get; set; }
	public double AvgFruitWeight { get; set; }
}