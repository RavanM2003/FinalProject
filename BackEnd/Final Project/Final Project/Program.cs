using Final_Project;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.Registration(config);

var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
