namespace Inplanticular.Garden_Service.Core.Models;

public class Garden {
	public Garden() {
	}

	public Garden(string name, string userId, DateTime dateOfCreation) {
		Id = Guid.NewGuid().ToString();
		Name = name;
		UserId = userId;
		DateOfCreation = dateOfCreation;
	}

	public string Id { get; set; }
	public string UserId { get; set; }
	public string Name { get; set; }
	public DateTime DateOfCreation { get; set; }
	public ICollection<Plant> Plants { get; }
}