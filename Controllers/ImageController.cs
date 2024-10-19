using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSite.Helpers;
using WebSite.Models;
using WebSite.Models.Dto;
using WebSite.Repositoryes.Interfaces;
using WebSite.Services.Interfaces;

namespace WebSite.Controllers
{
	[Route("api")]
	[Controller]
	public class ImageController : Controller
	{
		private readonly IRepository<ProductImage> _imageRepo;
		private readonly IRepository<Product> _productRepo;
		private readonly IImageService _imageService;

		public ImageController(IRepository<ProductImage> imageRepo, IRepository<Product> productRepo, IImageService imageService)
		{
			_imageRepo = imageRepo;
			_productRepo = productRepo;
			_imageService = imageService;
		}

		[HttpGet]
		[Route("all-images")]
		public IResult GetAll(int productId = -1)
		{
			if (productId < 0)
			{
				return Results.BadRequest();
			}

			var product = _productRepo.Get(productId);

			if (product == null)
			{
				return Results.NotFound();
			}

			var result = _imageRepo.Where(i => i.ProductId == productId);

			if (result == null)
			{
				return Results.NotFound();
			}

			return Results.Json(result);
		}

		[HttpGet]
		[Route("image")]
		public IResult Get(int id)
		{
			var image = _imageRepo.Get(id);

			if (image == null)
			{
				return Results.NotFound();
			}

			return Results.Bytes(_imageService.GetImage(image.ImagePath));
		}

		[HttpPost]
		[Route("image")]
		[Authorize(Roles = "Seller")]
		public IResult LoadImage([FromBody] ImageDto imageDto)
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			int sellerId = int.Parse(helper.GetValue("Id"));

			var product = _productRepo.Get(imageDto.ProductId);

			if (product == null)
			{
				return Results.BadRequest();
			}

			if (product.SellerId != sellerId)
			{
				return Results.BadRequest();
			}

			foreach (IFormFile file in imageDto.Files)
			{
				var productImage = new ProductImage();
				productImage.ProductId = imageDto.ProductId;
				productImage.ImagePath = $"{sellerId}/{imageDto.ProductId}/{file.FileName}";

				_imageService.LoadImage(productImage.ImagePath, file);
				_imageRepo.Add(productImage);
			}

			return Results.Ok();
		}

		[HttpDelete]
		[Route("image")]
		[Authorize(Roles = "Seller, Admin")]
		public IResult Delete(int id)
		{
			var image = _imageRepo.Get(id);

			if (image == null)
			{
				return Results.NotFound();
			}

			_imageService.DeleteImage(image.ImagePath);
			_imageRepo.Remove(id);

			return Results.Ok();
		}
	}
}
