using MeetingManage.CustomAuthorization;
using MeetingManage.Data;
using MeetingManage.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region 使整個網站預設都需要登入才能瀏覽
//builder.Services.AddMvc(config =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//                     .RequireAuthenticatedUser()
//                     .Build();
//    config.Filters.Add(new AuthorizeFilter(policy));
//});
#endregion

#region Migration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationContext")));
#endregion

#region Authentication Cookies實作

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{    
    options.LoginPath = "/Auth/LogIn"; //指定登入頁面
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(1);//cookie 過期時間
    options.AccessDeniedPath = new PathString("/Auth/unauthorized");
});
builder.Services.AddHttpContextAccessor();
#endregion




builder.Services.AddSingleton<JwtHelpers>();
builder.Services.AddScoped<userHelpers>();
builder.Services.AddScoped<TokenHelpers>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

#region Authentication Cookies實作
app.UseAuthentication();
app.UseAuthorization();
#endregion


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
