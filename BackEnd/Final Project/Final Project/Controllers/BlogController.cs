using Final_Project.DAL;
using Final_Project.ViewModels.BlogViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            BlogVM blogVM = new();
            blogVM.Blogs = _context.Blogs
                .Include(b=>b.AppUser)
                .Where(b=>!b.IsDeleted).ToList();
            return View(blogVM);
        }
        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            BlogVM blogVM = new();
            blogVM.Blog = _context.Blogs.Include(b => b.AppUser).Where(b=>!b.IsDeleted).FirstOrDefault(b=>b.Id ==  id);
            blogVM.Blogs = _context.Blogs
                .Include(b => b.AppUser)
                .Take(4)
                .Where(b => !b.IsDeleted && b.Id != id).ToList();
            blogVM.Sponsores = _context.Sponsores.Where(s=>!s.IsDeleted ).ToList();
            return View(blogVM);
        }
    }
}
