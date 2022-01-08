using System.IdentityModel.Tokens.Jwt;

namespace MeetingManage.Helpers
{
    public class TokenHelpers
    {
        public string GetUser(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var Claims = tokenHandler.ReadJwtToken(token).Payload.Claims;
            var Rusult = Claims.FirstOrDefault(x => x.Type == "sub").Value;
            return Rusult;
        }
        public string GetUserRole(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var Claims = tokenHandler.ReadJwtToken(token).Payload.Claims;
            var Rusult = Claims.FirstOrDefault(x => x.Type == "roles").Value;
            return Rusult;
        }
    }
}
