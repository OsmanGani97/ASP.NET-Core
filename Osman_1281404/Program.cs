using Microsoft.EntityFrameworkCore;
using Osman_1281404.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RestaurantDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();

//app.MapControllerRoute(
//    name: "myroute",
//    pattern: "{controller=Home}/{action=Index}/{id?}"
//    );

app.Run();
