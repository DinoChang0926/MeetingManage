using MeetingManage.CustomAuthorization;
using MeetingManage.Data;
using MeetingManage.Helpers;
using MeetingManage.Models;
using MeetingManage.ViewModels;
using MeetingManage.ViewModels.Meeting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MeetingManage.CustomAuthorization.Globals;
using static MeetingManage.ViewModels.RoomDataTable;

namespace MeetingManage.Controllers
{
    [UserAuthorization]
    public class MeetingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _ApplicationDB;
        private readonly TokenHelpers _tokenHelper;
        private readonly PageHelpers _pageHelper;
        public MeetingController(ApplicationDbContext ApplicationDB,
                                 IConfiguration configuration,
                                 PageHelpers pageHelper,
                                 TokenHelpers tokenHelps)
        {
            _ApplicationDB = ApplicationDB;
            _configuration = configuration;
            _tokenHelper = tokenHelps;
            _pageHelper = pageHelper;
        }
        readonly List<DrownItem> _MR = new List<DrownItem>
        {
                new DrownItem("第一會議室","第一會議室"),
                new DrownItem("第二會議室","第二會議室"),
                new DrownItem("第三會議室","第三會議室"),
                new DrownItem("第四會議室","第四會議室"),
                new DrownItem("第五會議室","第五會議室")
        };

        [AllowAnonymous]
        public IActionResult SearchMeeting(string SearchDate)
        {

            RoomObject obj = new RoomObject();
            if (SearchDate == null)
                obj.SearchDate = DateTime.Now.ToString("yyyy-MM-dd");  //搜尋日期為空值時設為當天                  
            else
                obj.SearchDate = SearchDate;
            obj.STime = obj.SearchDate +" "+ _configuration.GetValue<string>("MeetingObject:onDuty"); //設定Google Chart的X軸起始時間
            obj.ETime = obj.SearchDate +" "+ _configuration.GetValue<string>("MeetingObject:offDuty");  //設定Google Chart的X軸結束時間
            return View(obj);
        }


        public IActionResult List(string SearchDate,int? CurrentPage) //會議列表管理
        {
            if (SearchDate == null)
            {
                SearchDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            ViewBag.SearchDate = SearchDate;
            var r = _pageHelper.GetPage(GetMeeting(SearchDate), CurrentPage);
            ViewBag.Page = r.Item2;
            return View(r.Item1);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MeetingsViewModel req)
        {
            string token = HttpContext.Request.Cookies["token"];
            string userAccount = _tokenHelper.GetUser(token);
            try
            {               
                var result = MeetingCheck(req);             
                if (result.Item1)
                {                    
                    _ApplicationDB.Meetings.Add(new Meeting { 
                        Account = userAccount,
                        Applicant=req.Applicant,
                        STime= req.Date + " " + req.STime,
                        ETime= req.Date + " " + req.ETime,
                        Remarks=req.Remarks, 
                        Event   =req.Event,
                        Room    =req.Room,
                    });
                    _ApplicationDB.SaveChanges();
                    TempData["message"] = "新增成功";
                }
                else
                {
                    TempData["message"] = result.Item2;
                    return View(req);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "異常新增錯誤，請查閱系統紀錄";
                Console.WriteLine("{0} meeting create fail: account_id:{2} edit fail:{1}", DateTime.Now, ex.Message, userAccount);
                return View(req);
            }
            return RedirectToAction("List");
        }
        public IActionResult Edit(long Id) 
        {     
            try
            {
                Meeting meeting = _ApplicationDB.Meetings.FirstOrDefault(a => a.Id == Id);
                MeetingsViewModel viewModel = new MeetingsViewModel { 
                    Account =meeting.Account,
                    Date = DateTime.Parse(meeting.STime).ToString("yyyy-MM-dd"),
                    STime = DateTime.Parse(meeting.STime).ToString("HH:mm"),
                    ETime = DateTime.Parse(meeting.ETime).ToString("HH:mm"),
                    Applicant = meeting.Applicant,
                    Event =meeting.Event,
                    Remarks =meeting.Remarks,
                    Room =meeting.Room,
                    Id = Id,
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                string token = HttpContext.Request.Cookies["token"];
                string userAccount = _tokenHelper.GetUser(token);
                TempData["message"] = "錯誤請求";
                Console.WriteLine("{0} meeting edit fail: account_id:{2} edit fail:{1}", DateTime.Now, ex.Message, userAccount);
                return RedirectToAction("List");
            }  
               
        }
        [HttpPost]
        public IActionResult Edit(MeetingsViewModel req)
        {
            string token = HttpContext.Request.Cookies["token"];
            string userAccount = _tokenHelper.GetUser(token);
            try
            {
                var result = MeetingCheck(req);
                if (result.Item1)
                {
                    _ApplicationDB.Meetings.Update(new Meeting
                    {
                        Account = userAccount,
                        Applicant = req.Applicant,
                        STime = req.Date + " " + req.STime,
                        ETime = req.Date + " " + req.ETime,
                        Remarks = req.Remarks,
                        Event = req.Event,
                        Room = req.Room,
                        Id=req.Id,
                    });
                    _ApplicationDB.SaveChanges();
                    TempData["message"] = "更新成功";
                }
                else
                {
                    TempData["message"] = result.Item2;
                    return View(req);
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = "異常編輯錯誤，請查閱系統紀錄";
                Console.WriteLine("{0} meeting edit fail: account_id:{2} edit fail:{1}", DateTime.Now, ex.Message, userAccount);
                return View(req);
            }
            return RedirectToAction("List");        
        }
        [HttpPost]
        public IActionResult Delete(long Id)
        {           
            try
            {
                     Meeting meeting = _ApplicationDB.Meetings.FirstOrDefault(a => a.Id == Id);
                    _ApplicationDB.Meetings.Remove(meeting);
                    _ApplicationDB.SaveChanges();
                    TempData["message"] = "刪除成功";
            }
            catch (Exception ex)
            {
                string token = HttpContext.Request.Cookies["token"];
                string userAccount = _tokenHelper.GetUser(token);
                TempData["message"] = "異常刪除錯誤，請查閱系統紀錄";
                Console.WriteLine("{0} meeting delete fail: account_id:{2} edit fail:{1}", DateTime.Now, ex.Message, userAccount);             
            }
            return RedirectToAction("List");
        }

        #region Helper
        public List<DrownItem> GetRoom()
        {
            return _MR;
        }
        private List<Meeting> GetMeeting(string SearchDate)
        {
            List<Meeting> Meetings = _ApplicationDB.Meetings.Where(a => a.STime.Contains(SearchDate)).ToList();
            string token = HttpContext.Request.Cookies["token"];
            string userRole = _tokenHelper.GetUserRole(token);
            string userAccount = _tokenHelper.GetUser(token);
            if (byte.TryParse(userRole, out byte Role))
            {
                switch ((RoleType)Role)
                {
                    case RoleType.Admin:
                        break;

                    default:
                        Meetings = Meetings.Where(a => a.Account.Equals(userAccount)).ToList();
                        break;
                }
            }
            return Meetings;
        }

        [AllowAnonymous]
        public JsonResult MeetingDataToJson(string SearchDate) //處理給Google的Json數據
        {
            List<Meeting> MData = _ApplicationDB.Meetings.Where(x => x.STime.Contains(SearchDate)).OrderBy(x => x.Room).ToList();
            Graph graph = null;
            if (MData.Count() == 0)
                return this.Json(graph);
            graph = new Graph
            {
                cols = new List<Col>
                {
                 // id 可直接填空字串，預設會自動建立
                 // label 為提示訊息前置值，假如 沒有設定 f 資料，預設會讀取 label 加上此資料格的值 例如：小計:654.0600
                 // type 此欄資料格式，只有 boolean、number、string、date、datetime、timeofday 六種格式
                  new Col { id = "Room", label = "會議室", type = "string" },
                  new Col { id = "Event", label = "申請事由", type = "string" },
                  new Col { id = "STime", label = "起始時間", type = "date" },
                  new Col { id = "ETime", label = "結束時間", type = "date" }
                },
                rows = new List<DataPointSet>()
            };
            List<DataPointSet> rows = new List<DataPointSet>();
            foreach (Meeting mt in MData)
            {
                DataPointSet cSetRow = new DataPointSet();
                List<DataPoint> cSetList = new List<DataPoint>();
                DataPoint[] cSetProp = new DataPoint[5]; //將會議室數據轉為LIST
                cSetProp[0] = new DataPoint();
                cSetProp[0].v = mt.Room.Trim(' ');
                cSetList.Add(cSetProp[0]);
                cSetProp[1] = new DataPoint();
                cSetProp[1].v =/*"申請事由："+ mt.Event.Trim(' ')+*/" 申請人：" + mt.Applicant.Trim(' ');
                cSetList.Add(cSetProp[1]);
                cSetProp[2] = new DataPoint();
                cSetProp[2].v = Timehandle(mt.STime);
                cSetList.Add(cSetProp[2]);
                cSetProp[3] = new DataPoint();
                cSetProp[3].v = Timehandle(mt.ETime);
                cSetList.Add(cSetProp[3]);
                cSetRow.c = cSetList;
                rows.Add(cSetRow);
            }
            graph.rows = rows;
            //return this.Json(graph, JsonRequestBehavior.AllowGet);
            return Json(graph);
        }
        private (bool, string) MeetingCheck(MeetingsViewModel Meeting) //確認會議是否符合規則
        {
            int lim= _configuration.GetValue<int>("MeetingObject:lim"); //會議室使用最低間隔時間
            bool result = false;                                                                        
            if (Meeting == null)//確認會議室時間輸入無誤
            {
                return (result,"資料輸入有誤");
            }
            DateTime ts = DateTime.Parse(Meeting.Date + " " + Meeting.STime); //申請起始時間
            DateTime te = DateTime.Parse(Meeting.Date + " " + Meeting.ETime); //申請結束時間
            if (ts == te || ts > te)//確認會議室時間輸入無誤
            {
                return (result, "時間輸入有誤");
            }
            else if (DateTime.Parse(Meeting.Date + " " + Meeting.STime) < DateTime.Now)
            {
                return (result, "不可申請過去的時間");
            }
            IQueryable<Meeting> Mdata = _ApplicationDB.Meetings.Where(x => x.STime.Contains(Meeting.Date) && x.Room == Meeting.Room && x.Id != Meeting.Id);
            foreach (var m in Mdata)
            {
                DateTime TS = DateTime.Parse(m.STime); //已申請起始時間
                DateTime TE = DateTime.Parse(m.ETime); //已申請結束時間
                TimeSpan ResultSS = ts.Subtract(TS).Duration();//比對時間間隔
                TimeSpan ResultSE = ts.Subtract(TE).Duration();
                TimeSpan ResultES = te.Subtract(TS).Duration();
                if ((ResultSS.TotalMinutes <= lim || ResultSE.TotalMinutes < lim || ResultES.TotalMinutes < lim) ||  //確認不低於最低間隔時間
                    (ts < TS && te > TS || ts > TS && ts < TE))                                                      //確認會議室不重複使用
                {
                    return (result, "會議室已有人使用");
                }
           
            }
            result = true;
            return (result,"");
        }
        public string Timehandle(string s) //處理給Google 圖表的日期字串
        {
            string result = "Date(";
            string[] SplitSpace, Date, Time;
            string[] DateTime = new string[2];
            SplitSpace = s.Split(' ');
            for (int i = 0; i < SplitSpace.Length; i++)
            {
                DateTime[i] = SplitSpace[i];
            }
            Date = DateTime[0].Split('-');
            Time = DateTime[1].Split(':');
            Date[1] = (int.Parse(Date[1]) - 1).ToString(); //Google Chart的月份需比實際-1才會正確
            foreach (var r in Date)
            {
                result = result + r + ",";
            }
            foreach (var r in Time)
            {
                result = result + r + ",";
            }
            result = result.Remove(result.Length - 1, 1) + ")";
            return result;
        }
    }
    #endregion
}
