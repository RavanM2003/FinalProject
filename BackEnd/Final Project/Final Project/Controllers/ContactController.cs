using Final_Project.DAL;
using Final_Project.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ContactVM contactVM = new ContactVM();
            contactVM.Setting = _context.Settings.Where(s => !s.IsDeleted).AsNoTracking().ToDictionary(s => s.Key, s => s.Value);
            return View(contactVM);
        }
    }
}
