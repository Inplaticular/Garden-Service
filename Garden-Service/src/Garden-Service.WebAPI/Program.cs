using Inplanticular.Garden_Service.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Inplanticular.Garden_Service.WebAPI;

public class Program {
	public static async Task Main(string[] args) {
		var host = CreateHostBuilder(args).Build();
		
		using (var scope = host.Services.CreateScope()) {
			var gardenContext = scope.ServiceProvider.GetRequiredService<GardenContext>();
			var pendingMigrations = await gardenContext.Database.GetPendingMigrationsAsync();
			
			if (pendingMigrations.Any())
				await gardenContext.Database.MigrateAsync();
		}

		await host.RunAsync();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) {
		return Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
	}
}