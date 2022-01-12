using MeetingManage.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static MeetingManage.CustomAuthorization.Globals;

namespace MeetingManage.CustomAuthorization
{
    public class UserMangeAuthorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var controllerName = context.RouteData.Values["Controller"];
            //var actionName = context.RouteData.Values["Action"];
            string token = context.HttpContext.Request.Cookies["token"];
            if (!(context.HttpContext.User.Identity.IsAuthenticated && token != null))
            {                
                 // 判斷使用者是否登入         
                string path = $"~/Auth/Login";
                context.Result = new RedirectResult(path);
            }
            else
            {
                string CookieRole = context.HttpContext.User.Claims.FirstOrDefault().Value;
                if (!(userAuthorization(token, CookieRole)))
                {
                    // 判斷用戶權限        
                    string path = $"~/Auth/unauthorized";
                    context.Result = new RedirectResult(path);
                }
            }

        }
        private bool userAuthorization(string token,string cookieUserRole)
        {
            TokenHelpers tokenHelpers = new TokenHelpers();
            byte cUserRole;
            return byte.TryParse(cookieUserRole, out cUserRole) &&
                   tokenHelpers.GetUserRole(token).Equals(cookieUserRole) ? true : false;
        }
    }
}
