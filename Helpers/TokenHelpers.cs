using System.IdentityModel.Tokens.Jwt;

namespace MeetingManage.Helpers
{
    public class TokenHelpers
    {      
        private readonly IHttpContextAccessor _httpContext;
        public TokenHelpers()
        {
        }
        public TokenHelpers(IHttpContextAccessor httpContextAccessor)
        {            
               _httpContext = httpContextAccessor;
        }
        public string GetUser(string token = null) 
        {
            if (token == null)
                token = _httpContext.HttpContext.Request.Cookies["token"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var Claims = tokenHandler.ReadJwtToken(token).Payload.Claims;
            var Rusult = Claims.FirstOrDefault(x => x.Type == "sub").Value;
            return Rusult;
        }
        public string GetUserRole(string token = null)
        {
            if (token == null)
                token = _httpContext.HttpContext.Request.Cookies["token"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var Claims = tokenHandler.ReadJwtToken(token).Payload.Claims;
            var Rusult = Claims.FirstOrDefault(x => x.Type == "roles").Value;
            return Rusult;
        }

    }
}
