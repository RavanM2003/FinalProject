using Final_Project;
using Final_Project.Data;
using Final_Project.Hubs;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.Registration(config);

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=dashboard}/{action=Index}/{id?}"
          );

app.MapDefaultControllerRoute();
AppDbInitializer.Seed(app);
app.MapHub<ChatHub>("/chatHub");
app.Run();
