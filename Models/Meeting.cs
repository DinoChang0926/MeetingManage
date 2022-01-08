using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingManage.Models
{
    public class Meeting
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("user_id")]
        public long userId { get; set; }
        [Column("Room")]

        public string Room { get; set; }
        [Column("Event")]
        public string Event { get; set; }
        [Column("STime")]
        public string STime { get; set; }
        [Column("ETime")]
        public string ETime { get; set; }
        [Column("Remarks")]
        public string Remarks { get; set; }
        [Column("Account")]
        public string Account { get; set; }
        [ForeignKey(nameof(userId))]
        [InverseProperty("Meetings")]
        public virtual User User { get; set; }
    }
}
