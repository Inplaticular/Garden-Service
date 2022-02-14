using System.Collections;

namespace Inplanticular.Garden_Service.Core.Models; 

public class Garden {
	public string Name { get; set; }
	public DateTime DateOfCreation { get; set; }
	public List<Plant> Plants { get; set; }
	
}