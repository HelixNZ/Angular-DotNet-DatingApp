using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

			//Lifetime
			services.AddSingleton<PresenceTracker>(); //Presence
			services.AddScoped<ITokenService, TokenService>(); //JWT tokens
			services.AddScoped<IUnitOfWork, UnitOfWork>(); //Repo handler
			services.AddScoped<IPhotoService, PhotoService>(); //Cloudinary
			services.AddScoped<LogUserActivity>(); //Last Seen
			services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

			//Add database context
			services.AddDbContext<DataContext>(options =>
			{
				//Use Sqlite with the default connection specified in appsettings.dev
				options.UseSqlite(config.GetConnectionString("DefaultConnection"));
			});

            return services;
		}
	}
}