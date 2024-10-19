using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebSite.Extensions;
using WebSite.Helpers;
using WebSite.Models;
using WebSite.Models.Dto;
using WebSite.Options;
using WebSite.Repositoryes.Interfaces;
using WebSite.Services.Interfaces;

namespace WebSite.Controllers
{
	
	[Controller]
	[Route("customer")]
	public class CustomerController : Controller
	{
		private readonly IRepository<Customer> _customerRepo;
		private readonly IValidationService _validationService;
		private readonly IConfiguration _config;

		public CustomerController(IRepository<Customer> repository, IValidationService validationService, IConfiguration config)
		{
			_customerRepo = repository;
			_validationService = validationService;
			_config = config;
		}

		[HttpGet]
		[Route("/api/customer")]
		[Authorize(Roles = "Admin")]
		public IResult Get(int page = 1, int count = 100)
		{
			return Results.Json(_customerRepo.Where(s => true).Skip((page - 1) * count).Take(count));
		}

		[HttpGet]
		[Route("sign-in")]
		public void GetSignInPage()
		{
			HttpContext.Response.SendHtmlFile(_config.GetCustomerSignInPage());
		}

		[HttpGet]
		[Route("sign-up")]
		public void GetSignUpPage()
		{
			HttpContext.Response.SendHtmlFile(_config.GetCustomerSignUpPage());
		}

		[HttpPost]
		[Route("sign-in")]
		public IResult SignIn([FromBody] UserDto user)
		{
			if (!_validationService.ValidateUser(user))
			{
				return Results.BadRequest();
			}

			var customer = _customerRepo.Where(c => c.Email == user.Email & c.Password == user.Password).FirstOrDefault();

			if (customer == null)
			{
				return Results.Unauthorized();
			}

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, "Customer"),
				new Claim("Id", $"{customer.Id}")
			};

			var jwt = new JwtSecurityToken(
				issuer: AuthOptions.ISSUER,
				audience: AuthOptions.AUDIENCE,
				claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(14)),
				signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));


			var handler = new JwtSecurityTokenHandler();
			var token = handler.WriteToken(jwt);
			
			return Results.Ok("Bearer " + token);
		}

		[HttpPost]
		[Route("account")]
		public IResult CreateAccount([FromBody] Customer customer)
		{
			if (!_validationService.ValidateCustomer(customer))
			{
				return Results.BadRequest();
			}

			var res = _customerRepo.Where(c => c.Email == customer.Email).FirstOrDefault();

			if (res != null)
			{
				return Results.Conflict("User with this email already exists");
			}

			return _customerRepo.Add(customer);
		}

		[HttpDelete]
		[Route("account")]
		[Authorize(Roles = "Customer, Admin")]
		public IResult DeleteAccount(int id = -1)
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper.GetValue(ClaimTypes.Role) == "Customer")
			{
				id = int.Parse(helper.GetValue("Id"));
			}
			else if (helper.GetValue(ClaimTypes.Role) == "Admin" & id < 0)
			{
				return Results.BadRequest();
			}

			return _customerRepo.Remove(id);
		}
	}
}
