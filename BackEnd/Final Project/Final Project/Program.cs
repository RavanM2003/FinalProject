using Final_Project;
using Final_Project.Data;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.Registration(config);

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
AppDbInitializer.Seed(app);
app.Run();
