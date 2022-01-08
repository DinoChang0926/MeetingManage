using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MeetingManage.ViewModels
{
    public class RegisterViewModel 
    {
        [Required(ErrorMessage = "必填欄位")]
        [RegularExpression(@"^([^\<\>\'\%\'\#\'\&\'\*\'\$\'\@\'\\\'\?\'\!\'\.\)\(\&\+]*)$", ErrorMessage = "禁止使用特殊符號< > \\ % # $ # @ ? !")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "必填欄位")]
        //[RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=!]).*$", ErrorMessage = "必須輸入八個字符，並包含一個大寫字母、一個小寫字母、一個數字及一個特殊符號")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "必須輸入八個字符，並包含一個大寫字母、一個小寫字母和一個數字")]
        public string Password { get; set; }
        [Required(ErrorMessage = "必填欄位")]
        [Compare("Password", ErrorMessage = "與密碼不同。")]
        public string VerPassword { get; set; }
    }
}
