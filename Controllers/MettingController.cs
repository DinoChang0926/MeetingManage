using MeetingManage.Data;
using MeetingManage.Helpers;
using MeetingManage.Models;
using MeetingManage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static MeetingManage.ViewModels.RoomDataTable;

namespace MeetingManage.Controllers
{
    public class MeetingController : Controller
    {
        private readonly ApplicationDbContext _ApplicationDB;
        private readonly JwtHelpers _jwt;
        public MeetingController(ApplicationDbContext ApplicationDB, JwtHelpers jwt)
        {
            _ApplicationDB = ApplicationDB;
            _jwt = jwt;           
        }


        readonly List<DrownItem> MR = new List<DrownItem>
        {
                new DrownItem("第一會議室","第一會議室"),
                new DrownItem("第二會議室","第二會議室"),
                new DrownItem("第三會議室","第三會議室"),
                new DrownItem("第四會議室","第四會議室"),
                new DrownItem("第五會議室","第五會議室")
        };
        readonly List<DrownItem> LV = new List<DrownItem>
        {
                new DrownItem("管理者","admin"),
                new DrownItem("一般使用者","user")
        };
        private readonly int lim = 15; //會議室使用最低間隔時間    
      
     
  
        public ActionResult MeetingList(string SearchDate) //會議列表管理
        {
            //if (SearchDate == null)
            //{
            //    SearchDate = DateTime.Now.ToString("yyyy/MM/dd");
            //}
            //TempData["date"] = SearchDate;
            //IQueryable<Meeting> Mdata;
            //if (CheckLevel() == "admin")
            //{
            //    Mdata = _db.MeetingData.Where(x => x.STime.Contains(SearchDate)).AsQueryable().OrderBy(x => x.Room);
            //}
            //else
            //{
            //    Mdata = _db.MeetingData.Where(x => x.STime.Contains(SearchDate) && x.Account == account).AsQueryable().OrderBy(x => x.Room);  //一般使用者只查找自己帳號申請過的會議
            //}
            return View();
        }

        public ActionResult MeetingEdit(string SID) //會議編輯
        {
                return View();
        }

        public ActionResult MeetingDelete(long Id)  //會議刪除
        {
           
                return View();
            
          
        }
       
       
        [AllowAnonymous]
        public ActionResult SearchMeeting(string SearchDate)
        {
           
            RoomObject obj = new RoomObject();
            if (SearchDate == null)
                obj.SearchDate = DateTime.Now.ToString("yyyy/MM/dd");  //搜尋日期為空值時設為當天                  
            else
                obj.SearchDate = SearchDate;
            obj.STime = obj.SearchDate + " 08:00"; //設定Google Chart的X軸起始時間
            obj.ETime = obj.SearchDate + " 18:00";  //設定Google Chart的X軸結束時間
            return View(obj);
        }

        [AllowAnonymous]
        public JsonResult MeetingDataToJson(RoomObject Obj) //處理給Google的Json數據
        {
            IQueryable<Meeting> MData = _ApplicationDB.Meetings.Include(a=>a.User).Where(x => x.STime.Contains(Obj.SearchDate)).AsQueryable().OrderBy(x => x.Room);
            if (MData.Count() == 0)
                return this.Json("");
            Graph graph = new Graph
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
                cSetProp[1].v =/*"申請事由："+ mt.Event.Trim(' ')+*/" 申請人：" + mt.User.UserName.Trim(' ');
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
            return this.Json(graph);
        }
        public string MeetingCheck(MeetingsViewModel Meeting, long Id) //確認會議是否符合規則
        {
            if (Meeting == null)//確認會議室時間輸入無誤
            {
                return "資料輸入有誤";
            }
            DateTime ts = DateTime.Parse(Meeting.Date + " " + Meeting.STime); //申請起始時間
            DateTime te = DateTime.Parse(Meeting.Date + " " + Meeting.ETime); //申請結束時間
            if (ts == te || ts > te)//確認會議室時間輸入無誤
            {
                return "時間輸入有誤";
            }
            else if (DateTime.Parse(Meeting.Date + " " + Meeting.STime) < DateTime.Now)
            {
                return "不可申請過去的時間";
            }
            IQueryable<Meeting> Mdata = _ApplicationDB.Meetings.Where(x => x.STime.Contains(Meeting.Date) && x.Room == Meeting.Room && x.Id != Id);
            foreach (var m in Mdata)
            {
                DateTime TS = DateTime.Parse(m.STime); //已申請起始時間
                DateTime TE = DateTime.Parse(m.ETime); //已申請結束時間
                TimeSpan ResultSS = ts.Subtract(TS).Duration();//比對時間間隔
                TimeSpan ResultSE = ts.Subtract(TE).Duration();
                TimeSpan ResultES = te.Subtract(TS).Duration();
                if (ResultSS.TotalMinutes <= lim || ResultSE.TotalMinutes < lim || ResultES.TotalMinutes < lim) //確認不低於最低間隔時間
                {
                    return "會議室已有人使用";
                }
                else if (ts < TS && te > TS || ts > TS && ts < TE)  //確認時間是否重複
                {
                    return "會議室已有人使用";
                }
            }
            return "OK";
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
            Date = DateTime[0].Split('/');
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
}
