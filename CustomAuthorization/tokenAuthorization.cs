using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace MeetingManage.CustomAuthorization
{
    public class tokenAuthorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var controllerName = context.RouteData.Values["Controller"];
            //var actionName = context.RouteData.Values["Action"];
            string token = context.HttpContext.Request.Cookies["token"]; 
            // 判斷使用者是否登入            
            if (!(context.HttpContext.User.Identity.IsAuthenticated && token != null))
            {
                // 沒有登入 HttpStatusCode = 401 尚未授權                
                //context.Result = new RedirectResult($"~/{(int)HttpStatusCode.Unauthorized}");
                string path = $"~/Auth/login"; 
                context.Result = new RedirectResult(path);
            }
        }
        //檢查token時效
        private bool tokenExp(string token) 
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var c = tokenHandler.ReadJwtToken(token).Payload.Claims;       
            var Expires = Convert.ToInt32(c.SingleOrDefault(x => x.Type == "exp").Value);
            DateTimeOffset exp = DateTimeOffset.FromUnixTimeSeconds(Expires).ToOffset(new TimeSpan(8, 0, 0)); //token過期時間
            DateTimeOffset now = DateTime.Now;
            int r = DateTimeOffset.Compare(exp, now);
            return (DateTimeOffset.Compare(exp, now) > 0) ? true : false;
        }


    }
}




