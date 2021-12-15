namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{

		//Map photo to photo url as well
		CreateMap<AppUser, MemberDto>()
			.ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain && x.IsApproved).Url))
			//.ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Where(p => p.IsApproved))) //Can't do this as it kills it for the user who owns them
			.ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

		CreateMap<Photo, PhotoDto>();
		CreateMap<MemberUpdateDto, AppUser>();
		CreateMap<RegisterDto, AppUser>();

		//Message photo mapping
		CreateMap<Message, MessageDto>()
			.ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
			.ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
	}
}