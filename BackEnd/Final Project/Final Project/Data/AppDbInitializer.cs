using Final_Project.DAL;
using Microsoft.AspNetCore.Identity;

namespace Final_Project.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(new List<IdentityRole> {


                        new IdentityRole()
                        {
                            Name="Admin",
                            NormalizedName = "ADMIN"

                        },
                        new IdentityRole()
                        {
                            Name="User",
                            NormalizedName = "USER"
                        },
                        new IdentityRole()
                        {
                            Name="SuperAdmin",
                            NormalizedName = "SUPERADMIN"
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
