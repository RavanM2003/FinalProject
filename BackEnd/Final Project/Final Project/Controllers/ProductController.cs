using Final_Project.DAL;
using Final_Project.Models;
using Final_Project.ViewModels.ProductViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public ProductController(AppDbContext context, UserManager<AppUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        public IActionResult Detail(int id, string categoryName)
        {
            if (id == null) return NotFound();
            ProductVM productVM = new();
            productVM.Product = _context.Products
                .Where(c => !c.IsDeleted)
                .Include(p => p.ProductFeatures.Where(p=>!p.IsDeleted))
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(b => b.ProductComments)
                .ThenInclude(c => c.AppUser)
                .FirstOrDefault(p => p.Id == id);
            productVM.Products= _context.Products.Where(x=>x.CategoryId == 
            _context.Categories.FirstOrDefault(x=>x.Name == categoryName).Id && x.IsDeleted==false).ToList();
            return View(productVM);
        }
        public IActionResult ProductCategory(string name)
        {
            if (name == null) return NotFound();
            ProductVM productVM = new();
            productVM.Category = _context.Categories
                .Include(c=>c.Products)
                .Where(c=>!c.IsDeleted)
                .FirstOrDefault(c => c.Name == name);
            return View(productVM);
        }

        public IActionResult AddComment(string message, int Id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("login", "account");
            var user = _usermanager.FindByNameAsync(User.Identity.Name).Result;
            if (string.IsNullOrEmpty(message)) return NotFound();
            ProductComment comment = new();
            comment.Message = message;
            comment.ProductId = Id;
            comment.AppUserId = user.Id;
            comment.WriteTime = DateTime.Now;
            comment.AddedBy = User.Identity.Name;
            comment.CreatedTime = DateTime.Now;
            comment.IsDeleted = false;
            _context.ProductComments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("detail", new { id = Id });
        }
        public async Task<IActionResult> DeleteComment(int? Id)
        {
            if (Id == null)
            {
                return BadRequest("something went wrong");
            }
            var comment = _context.ProductComments.FirstOrDefault(p => p.Id == Id);
            if (comment == null)
            {
                return NotFound();
            }
            _context.ProductComments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("detail", new { id = comment.ProductId });
        }
    }
}
