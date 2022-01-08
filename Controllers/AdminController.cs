using MeetingManage.CustomAuthorization;
using MeetingManage.Data;
using MeetingManage.Helpers;
using MeetingManage.Models;
using MeetingManage.ViewModels;
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
        public AdminController(ApplicationDbContext ApplicationDB,
                               IConfiguration configuration,
                               TokenHelpers tokenHelps)
        {
            _ApplicationDB = ApplicationDB;
            _configuration = configuration;
            _tokenHelper = tokenHelps;
        }
        public ActionResult List(int? CurrentPage) 
        {
            List<AdminListViewModel> users = GetUsers().Select(x => new AdminListViewModel
                                                {
                                                    Account  = x.Account,
                                                    UserName = x.UserName,
                                                    Role     = ((RoleType) x.Role).ToString(),
                                                }).ToList();
            int pageSize = _configuration.GetValue<int>("PageSize");
            int currentPage = CurrentPage ?? 1;
            if (currentPage < 1)
                currentPage = 1;
            int totalRecords = users.Count();
            var skip = (currentPage - 1) * pageSize;
            users = users.Skip(skip).Take(pageSize).ToList();
            PageModel.PageResult<AdminListViewModel> page = new PageModel.PageResult<AdminListViewModel>(users)
            {
                currentPage = currentPage,
                pageSizes = pageSize,
                totalRecords = totalRecords,
                totalPage = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(pageSize))),
            };
            ViewBag.page = page;
            return View(users);
        }
        public ActionResult Create() 
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
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
            

    
    }
}
