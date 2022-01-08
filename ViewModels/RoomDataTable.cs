using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeetingManage.ViewModels
{
    public class RoomDataTable
    {
        public class Col //欄位定義
        {
            public string id { get; set; }
            public string label { get; set; }
            public string type { get; set; } // type 可接受格式類型:boolean、number、string、date、datetime、timeofday
        }
        public class DataPoint
        {
            public string v { get; set; }   // 圖表中 欄位數值
            public string f { get; set; }   // 圖表中 欄位提示訊息
        }
        public class DataPointSet
        {
            public List<DataPoint> c { get; set; }
        }
        public class Graph
        {
            public List<Col> cols { get; set; }
            public List<DataPointSet> rows { get; set; }
        }
        public class RoomObject
        {
            [Required(ErrorMessage = "請填入「查詢日期」")]
            [DisplayName("查詢日期")]
            public string SearchDate { get; set; }
            public string STime { get; set; }
            public string ETime { get; set; }
        }
    }
}
