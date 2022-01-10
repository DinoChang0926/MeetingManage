using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeetingManage.ViewModels.Admin
{
    public class AdminCreateViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "必填欄位")]
        [RegularExpression(@"^([^\<\>\'\%\'\#\'\&\'\*\'\$\'\@\'\\\'\?\'\!\'\.\)\(\&\+]*)$", ErrorMessage = "禁止使用特殊符號< > \\ % # $ # @ ? !")]
        [DisplayName("帳號")]
        public string Account { get; set; }
        [DisplayName("使用者名稱")]
        [Required(ErrorMessage = "必填欄位")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "必填欄位")]       
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "必須輸入八個字符，並包含一個大寫字母、一個小寫字母和一個數字")]
        [DisplayName("密碼")]
        public string Password { get; set; }
        [Required(ErrorMessage = "必填欄位")]
        [Compare("Password", ErrorMessage = "與密碼不同。")]
        [DisplayName("確認密碼")]
        public string VerPassword{get; set;}
        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("權限")]
        public byte Role { get; set; }
    }
}
