namespace API.Data;

public class UserRepository : IUserRepository
{
	private readonly DataContext _context;
	private readonly IMapper _mapper;

	public UserRepository(DataContext context, IMapper mapper)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task<MemberDto> GetMemberAsync(string username, bool isCurrentUser)
	{
		var query = _context.Users
			.Where(x => x.UserName == username)
			.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
			.AsQueryable();

		//If we are getting ourselves, ignore the query filter which prevents us from seeing unapproved photos
		if(isCurrentUser) query = query.IgnoreQueryFilters();

		return await query.FirstOrDefaultAsync();
	}

	public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
	{
		var query = _context.Users.AsQueryable();

		query = query.Where(u => u.UserName != userParams.CurrentUsername);
		query = query.Where(u => u.Gender == userParams.Gender);

		var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1); //minus one as may not have had bday yet, returning incorrect results
		var maxDob = DateTime.Today.AddYears(-userParams.MinAge + 1); //plus one otherwise 18 might be considered 17
		query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

		//new switch
		query = userParams.OrderBy switch
		{
			"created" => query.OrderByDescending(u => u.Created),
			_ => query.OrderByDescending(u => u.LastActive)
		};

		return await PagedList<MemberDto>.CreateAsync(
			query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(),
			userParams.PageNumber,
			userParams.PageSize);
	}

	public async Task<AppUser> GetUserByIdAsync(int id)
	{
		return await _context.Users.FindAsync(id);
	}

	public async Task<AppUser> GetUserByPhotoIdAsync(int id)
	{
		return await _context.Users
			.Include(p => p.Photos)
			.IgnoreQueryFilters()
			.Where(p => p.Photos.Any(p => p.Id == id))
			.FirstOrDefaultAsync();
	}

	public async Task<AppUser> GetUserByUsernameAsync(string username)
	{
		return await _context.Users
			.Include(p => p.Photos)
			.SingleOrDefaultAsync(x => x.UserName == username);
	}

	public async Task<string> GetUserGender(string username)
	{
		return await _context.Users
			.Where(x => x.UserName == username)
			.Select(x => x.Gender)
			.FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<AppUser>> GetUsersAsync()
	{
		return await _context.Users
			.Include(p => p.Photos)
			.ToListAsync();
	}

	public void Update(AppUser user)
	{
		//Mark user as having unsaved changes
		_context.Entry(user).State = EntityState.Modified;
	}
}