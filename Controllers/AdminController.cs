using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebSite.Options;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using WebSite.Extensions;
using WebSite.Models.Dto;

namespace WebSite.Controllers
{
    [Route("admin")]
    [Controller]
    public class AdminController : Controller
    {
		private readonly IConfiguration _config;

		public AdminController(IConfiguration config)
		{
			_config = config;
		}

		[HttpPost]
		[Route("sign-in")]
        public IResult SignIn([FromBody] UserDto dto)
        {
			if (dto.Password != _config.GetAdminPassword())
			{
				return Results.Unauthorized();
			}

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, "Admin")
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
    }
}
