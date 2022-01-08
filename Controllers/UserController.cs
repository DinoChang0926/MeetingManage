using MeetingManage.CustomAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManage.Controllers
{
    [Authorize(Roles = "User")]
    [tokenAuthorization]
    public class UserController : Controller
    {
        public ActionResult PasswordEdit() //密碼編輯
        {
          
            return View();
        }  

    }
}
