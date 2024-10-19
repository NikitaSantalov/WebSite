using Microsoft.AspNetCore.Http;

namespace WebSite.Models.Dto
{
	public class ImageDto : EntityDto
	{
		public FormFileCollection Files { get; set; } = new();
		public int ProductId { get; set; }
	}
}
