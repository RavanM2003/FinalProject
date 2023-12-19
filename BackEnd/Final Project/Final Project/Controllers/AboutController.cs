using Final_Project.DAL;
using Final_Project.ViewModels.About;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            AboutVM aboutVM = new AboutVM();
            aboutVM.Setting = _context.Settings.Where(s => !s.IsDeleted).AsNoTracking().ToDictionary(s => s.Key, s => s.Value);
            return View(aboutVM);
        }
    }
}
