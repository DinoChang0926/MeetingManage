using MeetingManage.CustomAuthorization;
using MeetingManage.Data;
using MeetingManage.Helpers;
using MeetingManage.Models;
using MeetingManage.ViewModels;
using MeetingManage.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MeetingManage.CustomAuthorization.Globals;

namespace MeetingManage.Controllers
{
   [UserMangeAuthorization]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _ApplicationDB;
        private readonly IConfiguration _configuration;
        private readonly TokenHelpers _tokenHelper;
        private readonly PageHelpers _pageHelper;
        private readonly UserHelpers _userHelper;
        public AdminController(ApplicationDbContext ApplicationDB,
                               IConfiguration configuration,
                               TokenHelpers tokenHelps,
                               PageHelpers pageHelper,
                               UserHelpers userHelper)
        {
            _ApplicationDB = ApplicationDB;
            _configuration = configuration;
            _tokenHelper = tokenHelps;
            _pageHelper = pageHelper;
            _userHelper = userHelper;
        }
        public ActionResult List(int? CurrentPage) 
        {
            List<AdminListViewModel> users = GetUsers().Select(x => new AdminListViewModel
                                                {
                                                    Id       = x.Id,
                                                    Account  = x.Account,
                                                    UserName = x.UserName,
                                                    Role     = ((RoleType) x.Role).ToString(),
                                                }).ToList();
            var r = _pageHelper.GetPage(users, CurrentPage);
            ViewBag.Page = r.Item2;
            return View(r.Item1);
        }
        public ActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAction(AdminCreateViewModel req)
        {
            string message = "";
            try
            {
                if (_ApplicationDB.Users.Where(a => a.Account == req.Account).ToList().Count == 0)
                {
                    _ApplicationDB.Users.Add(new Models.User
                    {
                        UserName = req.UserName,
                        Password = _userHelper.SetPassword(req.Password),
                        Account = req.Account,
                        Role = req.Role,
                    });
                    message = "新增成功";
                    _ApplicationDB.SaveChanges();
                }
                else
                    message = "新增失敗，帳號重複!!";
            } 
            catch (Exception ex)
            {
                Console.WriteLine("{0} account:{3} create fail:{1}", DateTime.Now,ex.Message,req.Account);
                message = "新增失敗，請查閱系統紀錄!!";
            }                

            TempData["message"] = message;   
            return RedirectToAction("List");
        }

        public ActionResult Edit(long Id)
        {
            User user = _ApplicationDB.Users.FirstOrDefault(a => a.Id == Id);
            AdminEditViewModel u = new AdminEditViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role,
            };
            return View(u);
        }
        [HttpPost]
        public ActionResult EditAction(AdminEditViewModel req)
        {
            string message = "";
            try
            {
                User user = _ApplicationDB.Users.FirstOrDefault(a => a.Id == req.Id);
                user.UserName = req.UserName != null ? req.UserName : user.UserName;
                user.Role = req.Role;
                user.Password = req.Password != null ? req.Password : user.Password;
                _ApplicationDB.Users.Update(user);
                _ApplicationDB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} account_id:{3} edit fail:{1}", DateTime.Now, ex.Message, req.Id);
                message = "修改失敗，請查閱系統紀錄!!";
            }
            TempData["message"] = message;
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult DeleteAction(long Id)
        {
            string message = "";
            try
            {
                User user = _ApplicationDB.Users.FirstOrDefault(a => a.Id == Id);    
                _ApplicationDB.Users.Remove(user);
                _ApplicationDB.SaveChanges();
                message = "刪除成功!";
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} account_id:{3} delete fail:{1}", DateTime.Now, ex.Message, Id);
                message = "刪除失敗，請查閱系統紀錄!!";
            }
            TempData["message"] = message;        
            return RedirectToAction("List");
        }
        private List<User> GetUsers()
        {
            List<User> users = _ApplicationDB.Users.ToList();
            string token = HttpContext.Request.Cookies["token"];
            string userRole = _tokenHelper.GetUserRole(token);
            if (byte.TryParse(userRole, out byte Role))
            {
                switch ((RoleType)Role)
                {
                    case RoleType.Admin:
                        break;
                    case RoleType.UserManage:
                        users = users.Where(a => a.Role != (byte)RoleType.Admin).ToList();
                        break;
                    case RoleType.User:
                        users = users.Where(a => a.Role != (byte)RoleType.Admin && a.Role != (byte)RoleType.UserManage).ToList();
                        break;
                    default:
                        users = null;
                        break;
                }
            }
            return users;
        }

        public List<RoleTypeSelect> GetRole()
        {
            List<RoleType> roleTypes = Globals.ToList<RoleType>();
            List<RoleTypeSelect> roles = new List<RoleTypeSelect> { };
            foreach(var x in roleTypes)
            {
                roles.Add(new RoleTypeSelect(x.ToString(),(byte)x));
            }
            return roles;
        }   
    }
}
