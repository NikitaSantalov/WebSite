using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WebSite.Extensions;
using WebSite.Services.Interfaces;

namespace WebSite.Services
{
	public class LocalImageService : IImageService
	{
		private IConfiguration _config;

		public LocalImageService(IConfiguration config)
		{
			_config = config;
		}

		public byte[] GetImage(string path)
		{
			return File.ReadAllBytes($"{_config.GetImagesPath()}/{path}");
		}

		public async void LoadImage(string path, IFormFile formFile)
		{
			using (var fileStream = new FileStream($"{_config.GetImagesPath()}/{path}", FileMode.Create))
			{
				await formFile.OpenReadStream().CopyToAsync(fileStream);
			}
		}

		public void DeleteImage(string path)
		{
			File.Delete($"{_config.GetImagesPath()}/{path}");
		}
	}
}
