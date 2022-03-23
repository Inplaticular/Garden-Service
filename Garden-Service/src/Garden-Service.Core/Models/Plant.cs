﻿namespace Inplanticular.Garden_Service.Core.Models;

public class Plant {
	public Plant() {
	}

	public Plant(string plantDataId, string gardenId) {
		Id = Guid.NewGuid().ToString();
		PlantDataId = plantDataId;
		GardenId = gardenId;
	}

	public string Id { get; set; }
	public string GardenId { get; set; }

	//not often changed members
	public PlantData PlantData { get; set; }

	public string PlantDataId { get; set; }

	//often changed members
	//growth related
	public int TimeFromPlanting { get; } = 1;

	public int RipePercentage { get; } = 1;

	//yield related
	public double ActFruitCount { get; } = 0;
}