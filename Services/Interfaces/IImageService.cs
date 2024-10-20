using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSite.Services.Interfaces
{
	public interface IImageService
	{
		public Task<byte[]> GetImage(string path);
		public void LoadImage(string path, IFormFile formFile);
		public void DeleteImage(string path);
	}
}
