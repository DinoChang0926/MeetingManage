using Microsoft.EntityFrameworkCore;

namespace MeetingManage.Data
{    
    public class CheckDataBase
    {
        public static void EnsurePopulated(IApplicationBuilder app, IWebHostEnvironment _webHostEnvironment)
        {
            ApplicationDbContext _context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Console.WriteLine("檢查是否有資料庫");
            //判斷是否有資料庫
            if (_context.Database.EnsureCreated())
            {
                Console.WriteLine("尚未建立資料庫，開始建立資料庫");
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }     
        }
    }
}
