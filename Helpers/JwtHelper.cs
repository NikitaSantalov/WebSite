using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace WebSite.Helpers
{
    public class JwtHelper
    {
        private JwtSecurityToken _token;

        public JwtHelper(JwtSecurityToken token)
        {
            _token = token;
        }

        public static JwtHelper GetJwt(StringValues authorization)
        {
			var handler = new JwtSecurityTokenHandler();

            if (authorization != StringValues.Empty)
            {
				var token = handler.ReadJwtToken(authorization.ToString().Split(' ')[1]);
				return new JwtHelper(token);
			}

            return new JwtHelper(new JwtSecurityToken());
		}

        public string GetValue(string type)
        {
            return _token.Claims.Where(claim => claim.Type == type).FirstOrDefault()?.Value!;
        }
    }
}
