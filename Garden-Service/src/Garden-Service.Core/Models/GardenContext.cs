using Microsoft.EntityFrameworkCore;

namespace Inplanticular.Garden_Service.Core.Models; 

public class GardenContext : DbContext{
	public DbSet<Garden> Gardens { get; set; }
	public DbSet<Plant> Plants { get; set; }

	public string DbPath { get; }

	public GardenContext()
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = System.IO.Path.Join(path, "blogging.db");
	}

	// The following configures EF to create a Sqlite database file in the
	// special "local" folder for your platform.
	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseNpgsql($"Data Source={DbPath}");
}