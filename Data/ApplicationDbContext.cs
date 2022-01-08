using MeetingManage.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingManage.Data
{
    public class ApplicationDbContext : DbContext
    {
 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } 
        public DbSet<Meeting> Meetings { get; set; }
    }

}
