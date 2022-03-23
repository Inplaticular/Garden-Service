namespace Inplanticular.Garden_Service.Core.Models;

public class PlantData {
	public PlantData() {
	}

	public PlantData(string botanicalName, string friendlyName, int daysToMature, double growthPerDay,
		double avgFruitWeight) {
		BotanicalName = botanicalName;
		FriendlyName = friendlyName;
		DaysToMature = daysToMature;
		GrowthPerDay = growthPerDay;
		AvgFruitWeight = avgFruitWeight;
	}

	//ID
	public string BotanicalName { get; set; }

	//semi-final member. Not changed often
	//general
	public string FriendlyName { get; set; }


	//growth related
	public int DaysToMature { get; set; }

	public double GrowthPerDay { get; set; }

	//yield related
	public double AvgFruitWeight { get; set; }
}