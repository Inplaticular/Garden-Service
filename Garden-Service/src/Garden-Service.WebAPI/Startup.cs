using System.Reflection;
using Inplanticular.Garden_Service.Core.Models;
using Inplanticular.Garden_Service.Core.Options;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Inplanticular.Garden_Service.WebAPI;

public class Startup {
	public Startup(IConfiguration configuration) {
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }


	public void ConfigureServices(IServiceCollection services) {
		services.Configure<GatewayOptions>(Configuration.GetSection(GatewayOptions.AppSettingsKey));
		services.Configure<IdentityServiceOptions>(Configuration.GetSection(IdentityServiceOptions.AppSettingsKey));

		services.AddDbContext<GardenContext>(options =>
			options.UseNpgsql(Configuration.GetConnectionString("postgres"),
				b => b.MigrationsAssembly("Inplanticular.Garden-Service.WebAPI"))
		);
		services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddScoped<IPlantService, PlantService>();
		services.AddScoped<IGardenService, GardenService>();
		services.AddScoped<IIdentityService, IdentityService>();
		services.AddScoped<IGardenPermissionManagementService, GardenPermissionManagementService>();

		services.AddControllers().AddNewtonsoftJson(options =>
			options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(c => {
			c.SwaggerDoc("v1", new OpenApiInfo {
				Title = "Garden-Service",
				Version = "v1",
				Description =
					"Garden-Service of Inplanticular. Contains information about gardens and its plants",
				Contact = new OpenApiContact {
					Name = "Florian Korch",
					Email = "s0568195@htw-berlin.de",
					Url = new Uri("https://github.com/Inplaticular/Garden-Service")
				}
			});
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			c.IncludeXmlComments(xmlPath);
		});
	}

	public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostEnvironment) {
		applicationBuilder.UseRouting();
		if (webHostEnvironment.IsDevelopment()) {
			applicationBuilder.UseSwagger();
			applicationBuilder.UseSwaggerUI();
		}

		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		applicationBuilder.UseHttpsRedirection();

		applicationBuilder.UseCors(policyBuilder => policyBuilder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

		applicationBuilder.UseAuthorization();

		applicationBuilder.UseEndpoints(e => e.MapControllers());
	}
}