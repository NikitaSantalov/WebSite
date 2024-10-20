using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
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
		private readonly IDtoService<Customer, CustomerDto> _dtoService;

		public CustomerController(IRepository<Customer> repository, 
			IValidationService validationService, 
			IConfiguration config, IDtoService<Customer, CustomerDto> dtoService)
		{
			_customerRepo = repository;
			_validationService = validationService;
			_config = config;
			_dtoService = dtoService;
		}

		[HttpGet]
		[Route("/api/customer")]
		public async Task<IResult> Get(int id = -1, int page = 1, int count = 100)
		{
			if (id <= 0)
			{
				return Results.BadRequest();
			}

			var castomers = await _customerRepo.Where(s => s.Id == id);

			var res = castomers.Skip((page - 1) * count).Take(count).Select(_dtoService.ToDto);

			return Results.Json(res);
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
		public async Task<IResult> SignIn([FromBody] UserDto user)
		{
			Customer? customer = null;

			var authorization = HttpContext.Request.Headers.Authorization;

			if (!StringValues.IsNullOrEmpty(authorization))
			{
				var helper = JwtHelper.GetJwt(authorization);

				if (helper == null)
				{
					return Results.BadRequest();
				}

				int id = int.Parse(helper.GetValue("Id"));

				customer = await _customerRepo.Get(id);
			}
			else if (!_validationService.ValidateUser(user))
			{
				return Results.BadRequest();
			}
			else
			{
				var customers = await _customerRepo.Where(c => c.Email == user.Email & c.Password == user.Password);
				customer = customers.FirstOrDefault();
			}

			if (customer == null)
			{
				return Results.Unauthorized();
			}

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, "Customer"),
				new Claim("Id", $"{customer.Id}")
			};
			
			return Results.Ok("Bearer " + JwtHelper.CreateToken(claims));
		}

		[HttpPost]
		[Route("account")]
		public async Task<IResult> CreateAccount([FromBody] Customer customer)
		{
			if (!_validationService.ValidateCustomer(customer))
			{
				return Results.BadRequest();
			}

			var results = await _customerRepo.Where(c => c.Email == customer.Email);
			var res = results.FirstOrDefault();

			if (res != null)
			{
				return Results.Conflict("User with this email already exists");
			}

			return await _customerRepo.Add(customer);
		}

		[HttpDelete]
		[Route("account")]
		[Authorize(Roles = "Customer, Admin")]
		public async Task<IResult> DeleteAccount(int id = -1)
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper == null)
			{
				return Results.BadRequest();
			}

			if (helper.GetValue(ClaimTypes.Role) == "Customer")
			{
				id = int.Parse(helper.GetValue("Id"));
			}
			else if (helper.GetValue(ClaimTypes.Role) == "Admin" & id < 0)
			{
				return Results.BadRequest();
			}

			return await _customerRepo.Remove(id);
		}
	}
}
