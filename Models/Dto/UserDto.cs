namespace WebSite.Models.Dto
{
	public class UserDto : EntityDto
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
