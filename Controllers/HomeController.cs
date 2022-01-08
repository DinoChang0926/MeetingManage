using MeetingManage.CustomAuthorization;
using MeetingManage.Helpers;
using MeetingManage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace MeetingManage.Controllers
{
    [tokenAuthorization]
    public class HomeController : Controller
    {       
        private readonly JwtHelpers _jwt;
        private readonly TokenHelpers _tokenHelpers;
        public HomeController(JwtHelpers jwt, TokenHelpers tokenHelpers)
        {          
            _jwt = jwt;
            _tokenHelpers = tokenHelpers;
        }
    
        public IActionResult Index()
        {
            string token = HttpContext.Request.Cookies["token"];
            var a= _tokenHelpers.GetUserRole(token);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}