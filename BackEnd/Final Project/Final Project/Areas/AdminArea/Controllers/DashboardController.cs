using Final_Project.DAL;
using Final_Project.ViewModels.Dahboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.Workers = _context.Workers.Where(x=>!x.IsDeleted).ToList();
            dashboardVM.Sales = _context.Sales
                .Include(s=>s.AppUser)
                .OrderByDescending(s=>s.Total)
                .Take(10)
                .ToList();
            return View(dashboardVM);
        }
    }
}
