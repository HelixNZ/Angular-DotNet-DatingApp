namespace API.Interfaces;

public interface IPhotoRepository
{
	Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
	Task<Photo> GetPhotoByIdAsync(int id);
	void RemovePhoto(Photo photo);

}