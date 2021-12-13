namespace API.Extensions;

public static class IdentityServiceExtensions
{
	public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
	{
		//Login configuration and requirements
		services.AddIdentityCore<AppUser>(opt =>
		{
			opt.Password.RequireNonAlphanumeric = false;
		})
			.AddRoles<AppRole>()
			.AddRoleManager<RoleManager<AppRole>>()
			.AddSignInManager<SignInManager<AppUser>>()
			.AddRoleValidator<RoleValidator<AppRole>>()
			.AddEntityFrameworkStores<DataContext>();

		//Add auth requirements
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
					ValidateIssuer = false, //API
						ValidateAudience = false, //Angular App
					};

				options.Events = new JwtBearerEvents
				{
						//SignalR auth
						OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];
						var path = context.HttpContext.Request.Path;

						if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
						{
							context.Token = accessToken;
						}

						return Task.CompletedTask;
					}
				};
			});

		services.AddAuthorization(opt =>
		{
			opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
			opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
		});

		return services;
	}
}