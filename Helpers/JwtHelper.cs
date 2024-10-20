using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using WebSite.Options;

namespace WebSite.Helpers
{
    public class JwtHelper
    {
        private JwtSecurityToken _token;

        public JwtHelper(JwtSecurityToken token)
        {
            _token = token;
        }

        public static JwtHelper? GetJwt(StringValues authorization)
        {
			var handler = new JwtSecurityTokenHandler();

            if (!StringValues.IsNullOrEmpty(authorization))
            {
                var jwt = authorization.ToString().Replace("\"", "").Split(' ')[1];
				var token = handler.ReadJwtToken(jwt);
				return new JwtHelper(token);
			}

            return null;
		}

        public static string CreateToken(ICollection<Claim> claims)
        {
			var jwt = new JwtSecurityToken(
				issuer: AuthOptions.ISSUER,
				audience: AuthOptions.AUDIENCE,
				claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
				signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

			var handler = new JwtSecurityTokenHandler();
			return handler.WriteToken(jwt);
		}

        public string GetValue(string type)
        {
            return _token.Claims.Where(claim => claim.Type == type).FirstOrDefault()?.Value!;
        }
    }
}
