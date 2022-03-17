using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Inplanticular.Garden_Service.WebAPI;

public class Startup {
	public Startup(IConfiguration configuration) {
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }


	public void ConfigureServices(IServiceCollection services) {
		services.AddDbContext<GardenContext>(options =>
			options.UseNpgsql(Configuration.GetConnectionString("postgres"),
				b => b.MigrationsAssembly("Inplanticular.Garden-Service.WebAPI"))
		);

		services.AddScoped<IPlantService, PlantService>();

		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
	}

	public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostEnvironment) {
		applicationBuilder.UseRouting();
		if (webHostEnvironment.IsDevelopment()) {
			applicationBuilder.UseSwagger();
			applicationBuilder.UseSwaggerUI();
		}

		applicationBuilder.UseHttpsRedirection();

		applicationBuilder.UseAuthorization();

		applicationBuilder.UseEndpoints(e => e.MapControllers());
	}
}