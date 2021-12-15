namespace API.Entities;

[Table("Photos")]
public class Photo
{
	public int Id { get; set; }
	public string Url { get; set; }
	public bool IsMain { get; set; } = false; //Defaut to false, in line with IsApproved being added
	public bool IsApproved { get; set; } = false; //Require approval before anyone can see it on the site
	public string PublicId { get; set; }
	public AppUser AppUser { get; set; }
	public int AppUserId { get; set; }
}