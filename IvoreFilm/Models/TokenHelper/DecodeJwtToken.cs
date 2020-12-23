using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace IvoreFilm.Helpers.TokenHelper
{
    public class DecodeJwtToken
    {
        public string ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.ReadToken(token) is JwtSecurityToken tokenS)
            {
                var keycloakId = tokenS.Claims.Single(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;
                return keycloakId;
            }

            return null;
        }

        public string GetRole(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.ReadToken(token) is JwtSecurityToken tokenS)
            {
                var role = tokenS.Claims.Single(x => x.Type.Equals("roles")).Value;
                return role;
            }

            return null;
        }
    }
}