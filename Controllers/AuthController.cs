using MeetingManage.CustomAuthorization;
using MeetingManage.Data;
using MeetingManage.Helpers;
using MeetingManage.Models;
using MeetingManage.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static MeetingManage.CustomAuthorization.Globals;

namespace MeetingManage.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _ApplicationDB;
        private readonly JwtHelpers _jwt;
        private readonly UserHelpers _user;
        private readonly IConfiguration _configuration;
        public AuthController(ApplicationDbContext ApplicationDB, 
                              JwtHelpers jwt, 
                              IConfiguration configuration, 
                              UserHelpers user)
        {
            _ApplicationDB = ApplicationDB;
            _configuration = configuration;
            _jwt = jwt;                  
            _user = user;
        }
        public IActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl; 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginActionAsync(LoginViewModel request, string ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _ApplicationDB.Users.FirstOrDefault(a => a.UserName == request.UserName);                
                    if (user != null && _user.VerPassword(user.Password,request.Password))
                    {                       
                        var claims = new List<Claim>
                            {
                                //new Claim(ClaimTypes.Name, "UerName"),
                                new Claim(ClaimTypes.Role, user.Role.ToString())
                            };
                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties { };
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        HttpContext.Response.Cookies.Append("token", _jwt.GenerateToken(user));
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "帳號或密碼錯誤";
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View("Login");
        }      

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult RegisterAction(RegisterViewModel register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ApplicationDB.Users.Add(new User
                    {
                        Password = _user.SetPassword(register.Password),
                        Account = register.UserName,
                        UserName = register.UserName,
                        Role = 0
                    });
                    _ApplicationDB.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

            return RedirectToAction("Login");
        }
        public IActionResult unauthorized()
        {
            return View();
        }



    }
}
