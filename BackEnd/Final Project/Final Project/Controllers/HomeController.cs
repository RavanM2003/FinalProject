using Final_Project.DAL;
using Final_Project.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new();
            homeVM.Sliders = _context.Sliders.Where(s=>!s.IsDeleted).ToList();
            homeVM.Sponsores = _context.Sponsores.Where(s=>!s.IsDeleted).ToList();
            homeVM.Workers = _context.Workers.Where(s=>!s.IsDeleted).OrderByDescending(w => w.Experience).Take(3).ToList();
            homeVM.Banner = _context.Banners.Where(s=>!s.IsDeleted).OrderByDescending(b=>b.CreatedTime).FirstOrDefault();
            homeVM.Product = _context.Products
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages)
                .Include(p=>p.ProductFeatures)
                .Include(p=>p.ProductComments)
                .Where(p=>!p.IsDeleted)
                .ToList();
            homeVM.Categories = _context.Categories.Where(c => !c.IsDeleted).ToList();
            return View(homeVM);
        }
    }
}
