using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeetingManage.ViewModels
{
    public class MeetingsViewModel
    {
        public long Id { get; set; }
        [DisplayName("會議室")]
        public string Room { get; set; }
        [Required(ErrorMessage = "請填入「申請人姓名」")]
        [DisplayName("申請人")]
        public string Applicant { get; set; }
        [Required(ErrorMessage = "請填入「申請事由」")]
        [DisplayName("申請事由")]
        public string Event { get; set; }
        [Required(ErrorMessage = "請填入「申請日期」")]
        [DisplayName("申請日期")]
        public string Date { get; set; }
        [Required(ErrorMessage = "請填入「起始時間」")]
        [DisplayName("起始時間")]
        public string STime { get; set; }
        [Required(ErrorMessage = "請填入「結束時間」")]
        [DisplayName("結束時間")]
        public string ETime { get; set; }
        [DisplayName("備註")]
        public string Remarks { get; set; }

        [DisplayName("申請帳號")]
        public string Account { get; set; }
    }
}
