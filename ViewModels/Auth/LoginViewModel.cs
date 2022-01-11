using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeetingManage.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("帳號")]
        public string Account { get; set; }
        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("密碼")]
        public string Password { get; set; }
    }
}
