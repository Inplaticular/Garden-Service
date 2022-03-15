using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace Inplanticular.Garden_Service.Core.Models; 

public class Garden {
	public int GardenId { get; set; }
	public string Name { get; set; }
	public DateTime DateOfCreation { get; set; }
	public List<Plant> Plants { get; } = new();

}