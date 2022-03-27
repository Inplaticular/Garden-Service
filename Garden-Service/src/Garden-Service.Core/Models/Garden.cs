using Newtonsoft.Json;

namespace Inplanticular.Garden_Service.Core.Models;

public class Garden {
	public Garden() {
	}

	public Garden(string name, string userId, double coordinateLatitude, double coordinateLongitude,
		DateTime dateOfCreation) {
		Id = Guid.NewGuid().ToString();
		Name = name;
		UserId = userId;
		CoordinateLatitude = coordinateLatitude;
		CoordinateLongitude = coordinateLongitude;
		DateOfCreation = dateOfCreation;
	}

	public string Id { get; set; }
	public string UserId { get; set; }
	public string UnitId { get; set; }
	public string Name { get; set; }
	public double CoordinateLatitude { get; set; }
	public double CoordinateLongitude { get; set; }
	public DateTime DateOfCreation { get; set; }
	public ICollection<Plant> Plants { get; set; }

	public override string ToString() {
		return JsonConvert.SerializeObject(this);
	}
}