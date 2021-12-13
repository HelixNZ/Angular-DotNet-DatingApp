namespace API.Data;

public class Seed
{
	public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
	{
		if (await userManager.Users.AnyAsync()) return;

		//Use the seed json data to fill dummy accounts
		var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
		var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
		if (users == null) return;

		var roles = new List<AppRole>
			{
				new AppRole{Name = "Member"},
				new AppRole{Name = "Admin"},
				new AppRole{Name = "Moderator"},
			};

		foreach (var role in roles)
		{
			await roleManager.CreateAsync(role);
		}

		//Seed the user login information with basic information
		foreach (var user in users)
		{
			user.UserName = user.UserName.ToLower();
			await userManager.CreateAsync(user, "Pa$$w0rd");
			await userManager.AddToRoleAsync(user, "Member");
		}

		var admin = new AppUser
		{
			UserName = "admin"
		};

		await userManager.CreateAsync(admin, "Pa$$w0rd");
		await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
	}
}