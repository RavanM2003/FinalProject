using Final_Project.DAL;
using Final_Project.Models;
using Final_Project.ViewModels.ProductViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int id, string categoryName)
        {
            if (id == null) return NotFound();
            ProductVM productVM = new();
            productVM.Product = _context.Products
                .Where(c => !c.IsDeleted)
                .Include(p => p.ProductFeatures)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(b => b.ProductComments)
                .ThenInclude(c => c.AppUser)
                .FirstOrDefault(p => p.Id == id);
            productVM.Products= _context.Products.Where(x=>x.CategoryId == 
            _context.Categories.FirstOrDefault(x=>x.Name == categoryName).Id).ToList();
            return View(productVM);
        }
    }
}
