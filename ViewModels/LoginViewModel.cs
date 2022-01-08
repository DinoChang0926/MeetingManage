using System.ComponentModel.DataAnnotations;

namespace MeetingManage.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "必填欄位")]      
        public string UserName { get; set; }
        [Required(ErrorMessage = "必填欄位")]      
        public string Password { get; set; }
    }
}
