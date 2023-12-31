using Final_Project.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class HelpController : Controller
    {
        private readonly AppDbContext _context;

        public HelpController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var questions = _context.HelpQuestions.Where(h=>!h.IsDeleted).ToList();
            return View(questions);
        }
    }
}
