namespace Inplanticular.Garden_Service.Core.Models;

public class Plant {
	public Plant() {
	}

	public Plant(string plantDataId, string gardenId) {
		Id = Guid.NewGuid().ToString();
		PlantDataId = plantDataId;
		GardenId = gardenId;
	}

	public Garden Garden { get; set; }
	public string Id { get; set; }
	public string GardenId { get; set; }
	public string UnitId { get; set; }

	//not often changed members
	public PlantData PlantData { get; set; }

	public string PlantDataId { get; set; }

	//often changed members
	//growth related
	public int DaysToMature { get; set; }
	public int TimeFromPlanting { get; set; } = 1;

	public double GrowthPercentage { get; set; } = 1;

	public double Yield { get; set; } = 0;

	//yield related
	public int ActFruitCount { get; set; } = 0;
}