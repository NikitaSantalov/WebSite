using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebSite.Models;
using WebSite.Options;
using WebSite.Repositoryes.Interfaces;
using System.Linq;
using WebSite.Models.Dto;
using WebSite.Helpers;
using WebSite.Services.Interfaces;

namespace WebSite.Controllers
{
    [Route("seller")]
    [Controller]
    public class SellerController : Controller
    {
		private readonly IRepository<Seller> _sellerRepo;
		private readonly IValidationService _validationService;

		public SellerController(IRepository<Seller> repository, IValidationService validationService)
		{
			_sellerRepo = repository;
			_validationService = validationService;
		}

		[HttpGet]
		[Route("sign-in")]
		public void GetSignInPage()
		{
			HttpContext.Response.WriteAsync("<h1>Manufacturer Sign in</h1>");
		}

		[HttpGet]
		[Route("/api/seller")]
		[Authorize(Roles = "Admin")]
		public IResult Get(int page = 1, int count = 100)
		{
			return Results.Json(_sellerRepo.Where(s => true).Skip((page - 1) * count).Take(count));
		}

		[HttpGet]
		[Route("sign-up")]
		public void GetSignUpPage()
		{
			HttpContext.Response.WriteAsync("<h1>Manufacturer Sign up</h1>");
		}

		[HttpPost]
		[Route("account")]
		public IResult CreateAccount([FromBody] Seller seller)
		{
			if (!_validationService.ValidateSeller(seller))
			{
				return Results.BadRequest();
			}

			return _sellerRepo.Add(seller);
		}

		[HttpDelete]
		[Route("account")]
		[Authorize(Roles = "Seller, Admin")]
		public IResult DeleteAccount(int id = -1)
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper.GetValue(ClaimTypes.Role) == "Seller")
			{
				id = int.Parse(helper.GetValue("Id"));
			}
			else if (id < 0 & helper.GetValue(ClaimTypes.Role) == "Admin")
			{
				return Results.BadRequest();
			}

			return _sellerRepo.Remove(id);
		}

		[HttpPost]
		[Route("sign-in")]
		public IResult SignIn([FromBody] UserDto user)
		{
			if (!_validationService.ValidateUser(user))
			{
				return Results.BadRequest();
			}

			var seller = _sellerRepo.Where(s => s.Email == user.Email & s.Password == user.Password).FirstOrDefault();

			if (seller == null)
			{
				return Results.Unauthorized();
			}

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, "Seller"),
				new Claim("Id", $"{seller.Id}")
			};

			var jwt = new JwtSecurityToken(
				issuer: AuthOptions.ISSUER,
				audience: AuthOptions.AUDIENCE,
				claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
				signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

			var handler = new JwtSecurityTokenHandler();
			var token = handler.WriteToken(jwt);

			return Results.Ok("Bearer " + token);
		}
	}
}
