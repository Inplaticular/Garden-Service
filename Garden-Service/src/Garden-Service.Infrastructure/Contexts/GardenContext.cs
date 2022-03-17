using Microsoft.EntityFrameworkCore;

namespace Inplanticular.Garden_Service.Core.Models;

public class GardenContext : DbContext {
	public GardenContext(DbContextOptions options) : base(options) {
	}

	public DbSet<Garden> Gardens { get; set; }
	public DbSet<Plant> Plants { get; set; }
	
}