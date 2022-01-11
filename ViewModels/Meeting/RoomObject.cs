using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeetingManage.ViewModels
{
    public class RoomObject
    {
        [Required(ErrorMessage = "請填入「查詢日期」")]
        [DisplayName("查詢日期")]
        public string SearchDate { get; set; }
        public string STime { get; set; }
        public string ETime { get; set; }
    }
}
