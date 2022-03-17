namespace Inplanticular.Garden_Service.Core.Models;

public class Plant {
	public Plant() {
	}

	public Plant(string friendlyName, string botanicalName, int daysToMature, double growthPerDay,
		double avgFruitWeight) {
		FriendlyName = friendlyName;
		BotanicalName = botanicalName;
		DaysToMature = daysToMature;
		GrowthPerDay = growthPerDay;
		AvgFruitWeight = avgFruitWeight;
	}

	public int PlantId { get; set; }

	//semi-final member. Not changed often
	//general
	public string FriendlyName { get; set; }

	public string BotanicalName { get; set; }

	//growth related
	public int DaysToMature { get; set; }

	public double GrowthPerDay { get; set; }

	//yield related
	public double AvgFruitWeight { get; set; }

	//often changed members
	//growth related
	public int TimeFromPlanting { get; } = 1;

	public int RipePercentage { get; } = 1;

	//yield related
	public double ActFruitCount { get; } = 0;
}