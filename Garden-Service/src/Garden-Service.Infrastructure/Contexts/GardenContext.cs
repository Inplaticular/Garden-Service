using Microsoft.EntityFrameworkCore;

namespace Inplanticular.Garden_Service.Core.Models;

public class GardenContext : DbContext {
	public GardenContext(DbContextOptions options) : base(options) {
	}

	public DbSet<Garden> Gardens { get; set; }
	public DbSet<Plant> Plants { get; set; }

	public DbSet<PlantData> PlantData { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Garden>(entity => { entity.HasKey(garden => garden.Id); });

		modelBuilder.Entity<Plant>(entity => {
			entity.HasKey(plant => plant.Id);

			entity.HasOne<Garden>()
				.WithMany(garden => garden.Plants)
				.HasForeignKey(plant => plant.GardenId);

			entity.HasOne(plant => plant.PlantData).WithMany().HasForeignKey(plant => plant.PlantDataId);
		});

		modelBuilder.Entity<PlantData>(entity => { entity.HasKey(plantData => plantData.BotanicalName); });
	}
}