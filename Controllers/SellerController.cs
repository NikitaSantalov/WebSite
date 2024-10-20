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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebSite.Controllers
{
    [Route("seller")]
    [Controller]
    public class SellerController : Controller
    {
		private readonly IRepository<Seller> _sellerRepo;
		private readonly IValidationService _validationService;
		private readonly IDtoService<Seller, SellerDto> _dtoService;

		public SellerController(IRepository<Seller> repository, IValidationService validationService,
			IDtoService<Seller, SellerDto> dtoService)
		{
			_sellerRepo = repository;
			_validationService = validationService;
			_dtoService = dtoService;
		}

		[HttpGet]
		[Route("sign-in")]
		public void GetSignInPage()
		{
			HttpContext.Response.WriteAsync("<h1>Manufacturer Sign in</h1>");
		}


		[HttpGet]
		[Route("sign-up")]
		public void GetSignUpPage()
		{
			HttpContext.Response.WriteAsync("<h1>Manufacturer Sign up</h1>");
		}

		[HttpGet]
		[Route("/api/seller")]
		public async Task<IResult> Get(int id = -1, int page = 1, int count = 100)
		{
			if (page <= 0 | count <= 0)
			{
				return Results.BadRequest();
			}

			var castomers = await _sellerRepo.Where(s => s.Id == id);

			var res = castomers.Skip((page - 1) * count).Take(count).Select(_dtoService.ToDto);

			return Results.Json(res);
		}

		[HttpPost]
		[Route("account")]
		public async Task<IResult> CreateAccount([FromBody] Seller seller)
		{
			if (!_validationService.ValidateSeller(seller))
			{
				return Results.BadRequest();
			}

			var results = await _sellerRepo.Where(c => c.Email == seller.Email);
			var res = results.FirstOrDefault();

			if (res != null)
			{
				return Results.Conflict("User with this email already exists");
			}

			return await _sellerRepo.Add(seller);
		}

		[HttpDelete]
		[Route("account")]
		[Authorize(Roles = "Seller, Admin")]
		public async Task<IResult> DeleteAccount(int id = -1)
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper == null)
			{
				return Results.BadRequest();
			}

			if (helper.GetValue(ClaimTypes.Role) == "Seller")
			{
				id = int.Parse(helper.GetValue("Id"));
			}
			else if (id < 0 & helper.GetValue(ClaimTypes.Role) == "Admin")
			{
				return Results.BadRequest();
			}

			return await _sellerRepo.Remove(id);
		}

		[HttpPost]
		[Route("sign-in")]
		public async Task<IResult> SignIn([FromBody] UserDto user)
		{
			if (!_validationService.ValidateUser(user))
			{
				return Results.BadRequest();
			}

			var sellers = await _sellerRepo.Where(s => s.Email == user.Email & s.Password == user.Password);
			var seller = sellers.FirstOrDefault();

			if (seller == null)
			{
				return Results.Unauthorized();
			}

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, "Seller"),
				new Claim("Id", $"{seller.Id}")
			};

			return Results.Ok("Bearer " + JwtHelper.CreateToken(claims));
		}
	}
}
