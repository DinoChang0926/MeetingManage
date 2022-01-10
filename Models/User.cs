using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingManage.Models
{
    [Table("UserData")]
    public class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("Account")]
        public string Account { get; set; }
        [Column("userName")]
        public string UserName { get; set; }  
        [Column("password")]
        public string Password { get; set; }
        [Column("Role")]
        public byte Role { get; set; }
    }
}
