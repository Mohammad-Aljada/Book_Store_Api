using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Book_Store_API.Helper
{
    public class ExtractClaims
    {
        public static int? EtractUserId(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var JwtToken = tokenHandler.ReadJwtToken(token);

                var userIdCalim = JwtToken.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier);
                if (userIdCalim != null && int.TryParse(userIdCalim.Value, out int userId))
                {
                    return userId;
                }
                return null;

            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
