﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSite.Helpers;
using WebSite.Models;
using WebSite.Repositoryes.Interfaces;
using WebSite.Services.Interfaces;

namespace WebSite.Controllers
{
	[Route("api")]
	[Controller]
	public class ProductController : Controller
	{
		private readonly IRepository<Product> _productRepo;
		private readonly IRepository<Seller> _sellerRepo;
		private readonly IRepository<ProductImage> _imageRepo;
		private readonly IValidationService _validationService;

		public ProductController(IRepository<Product> productRepo,
								 IRepository<Seller> sellerRepo,
								 IRepository<ProductImage> imageRepo,
								 IValidationService validationService)
		{
			_productRepo = productRepo;
			_sellerRepo = sellerRepo;
			_imageRepo = imageRepo;
			_validationService = validationService;
		}

		[HttpGet]
		[Route("product")]
		public async Task<IResult> Get(string name = "", string category = "", int size = 0, decimal minCost = 0, decimal maxCost = 10000000, int page = 1, int count = 100)
		{
			var result = await _productRepo.Where(product =>
				((product.Name.ToLower().Contains(name.ToLower()) & product.Category == category & product.Size == size) |
				(product.Name.ToLower().Contains(name.ToLower()) & product.Category == category & size == 0) |
				(product.Name.ToLower().Contains(name.ToLower()) & category == "" & size == 0) |
				(name == "" & product.Category == category & size == 0) |
				(name == "" & category == "" & product.Size == size) |
				(name == "" & category == "" & size == 0)) &
				(minCost <= product.Cost & product.Cost <= maxCost));

			result = result.Skip((page - 1) * count).Take(count).Distinct();

			if (result.Count() == 0)
			{
				return Results.NotFound();
			}

			return Results.Ok(result);
		}

		[HttpPost]
		[Route("product")]
		[Authorize(Roles = "Seller")]
		public async Task<IResult> Add([FromBody] Product product)
		{
			if (!_validationService.ValidateProduct(product))
			{
				return Results.BadRequest();
			}

			product.Date = DateOnly.FromDateTime(DateTime.Now).ToShortDateString();

			if (product == null)
			{
				return Results.BadRequest();
			}

			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper == null)
			{
				return Results.BadRequest();
			}

			product.SellerId = int.Parse(helper.GetValue("Id"));

			return await _productRepo.Add(product);
		}

		[HttpPatch]
		[Route("product")]
		[Authorize(Roles = "Seller")]
		public async Task<IResult> Update([FromBody] Product product)
		{
			if (!_validationService.ValidateProduct(product))
			{
				return Results.BadRequest();
			}

			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper == null)
			{
				return Results.BadRequest();
			}

			product.SellerId = int.Parse(helper.GetValue("Id"));

			return await _productRepo.Update(product);
		}

		[HttpDelete]
		[Route("product")]
		[Authorize(Roles = "Seller, Admin")]
		public async Task<IResult> Delete(int id)
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper == null)
			{
				return Results.BadRequest();
			}

			string role = helper.GetValue(ClaimTypes.Role);
			int sellerId = int.Parse(helper.GetValue("Id"));

			var product = await _productRepo.Get(id);

			if (product == null)
			{
				return Results.NotFound();
			}

			if (role == "Seller" & sellerId != product.SellerId)
			{
				return Results.Unauthorized();
			}

			return await _productRepo.Remove(id);
		}
	}
}
