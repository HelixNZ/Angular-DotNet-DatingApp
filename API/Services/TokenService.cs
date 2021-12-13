namespace API.Services;

public class TokenService : ITokenService
{
	private readonly SymmetricSecurityKey _key;
	private readonly UserManager<AppUser> _userManager;

	public TokenService(IConfiguration config, UserManager<AppUser> userManager)
	{
		_userManager = userManager;
		_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
	}

	public async Task<string> CreateToken(AppUser user)
	{
		//Add claims
		var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
			};

		var roles = await _userManager.GetRolesAsync(user);
		claims.AddRange(roles.Select(roles => new Claim(ClaimTypes.Role, roles)));

		//Create credentials
		var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); ;

		//Describe token
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.Now.AddDays(7),
			SigningCredentials = creds
		};

		//Create the token
		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);

		return (tokenHandler.WriteToken(token));
	}
}