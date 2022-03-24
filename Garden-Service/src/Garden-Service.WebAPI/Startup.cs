using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Options;
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
		services.Configure<GatewayOptions>(this.Configuration.GetSection(GatewayOptions.AppSettingsKey));
		services.Configure<IdentityServiceOptions>(this.Configuration.GetSection(IdentityServiceOptions.AppSettingsKey));

		services.AddDbContext<GardenContext>(options =>
			options.UseNpgsql(Configuration.GetConnectionString("postgres"),
				b => b.MigrationsAssembly("Inplanticular.Garden-Service.WebAPI"))
		);

		services.AddScoped<IPlantService, PlantService>();
		services.AddScoped<IGardenService, GardenService>();
		services.AddScoped<IIdentityService, IdentityService>();
		services.AddScoped<IGardenPermissionManagementService, GardenPermissionManagementService>();

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
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		applicationBuilder.UseHttpsRedirection();

		applicationBuilder.UseAuthorization();

		applicationBuilder.UseEndpoints(e => e.MapControllers());
	}
}