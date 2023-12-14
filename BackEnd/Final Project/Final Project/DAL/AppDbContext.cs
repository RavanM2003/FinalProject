using Final_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Workers> Workers { get; set; }
        public DbSet<Sponsore> Sponsores { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
