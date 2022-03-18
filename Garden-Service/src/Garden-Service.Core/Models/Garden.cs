namespace Inplanticular.Garden_Service.Core.Models;

public class Garden {

	public Garden() {
		
	}
	public Garden(string name, DateTime dateOfCreation) {
		Name = name;
		DateOfCreation = dateOfCreation;
	}

	public int GardenId { get; set; }
	public string Name { get; set; }
	public DateTime DateOfCreation { get; set; }
	public List<Plant> Plants { get; } = new();
}