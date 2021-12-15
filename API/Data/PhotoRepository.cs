namespace API.Data;

public class PhotoRepository : IPhotoRepository
{
	private readonly DataContext _context;

	public PhotoRepository(DataContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
	{
		return await _context.Photos
			.IgnoreQueryFilters()
			.Where(p => p.IsApproved == false)
			.Select(p => new PhotoForApprovalDto
			{
				Id = p.Id,
				Username = p.AppUser.UserName,
				Url = p.Url,
				IsApproved = p.IsApproved
			}).ToListAsync();
	}

	public async Task<Photo> GetPhotoByIdAsync(int id)
	{
		return await _context.Photos
				.IgnoreQueryFilters()
				.SingleOrDefaultAsync(p => p.Id == id);
	}

	public void RemovePhoto(Photo photo)
	{
		_context.Photos.Remove(photo);
	}
}