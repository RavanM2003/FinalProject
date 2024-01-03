using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminCategory;
using Final_Project.ViewModels.AdminSlider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public CategoryController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1, int take = 5)
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Categories = _context.Categories
                .Where(s => !s.IsDeleted)
                .Include(p=>p.Products.Where(c => !c.IsDeleted))
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.Categories.Where(p => !p.IsDeleted).Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<Category> pagination = new(categoryVM.Categories, pageCount, page);

            return View(pagination);
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var existelemnt = _context.Categories.FirstOrDefault(s => s.Id == id);
            existelemnt.IsDeleted = true;
            existelemnt.DeletedTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            var existElement = _context.Categories
                .Where(c=>!c.IsDeleted)
                .Include(x=>x.Products.Where(c => !c.IsDeleted))
                .FirstOrDefault(x => x.Id == id);
            return View(existElement);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateCategoryVM createCategoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (!createCategoryVM.Photo.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }
            else if (!createCategoryVM.Photo.IsLenghSuit(1000))
            {
                ModelState.AddModelError("Image", "Length of file must be smaller than 1kb");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + createCategoryVM.Photo.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createCategoryVM.Photo.CopyTo(stream);
            }
            Category category = new()
            {
                ImageUrl = fileName,
                Name = createCategoryVM.Name,
                AddedBy = User.Identity.Name
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();
            var existItem = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();
            UpdateCategoryVM updateCategoryVM = new UpdateCategoryVM();
            updateCategoryVM.Name = existItem.Name;
            return View(updateCategoryVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, UpdateCategoryVM updateCategoryVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existItem = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();

            if (updateCategoryVM.Photo == null)
            {
                ModelState.AddModelError("", "Please enter the slider photo");
                return View(updateCategoryVM);
            }
            if (!updateCategoryVM.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Please choose the image file");
                return View(updateCategoryVM);
            }
            if (updateCategoryVM.Photo.Length / 1024 > 5000)
            {
                ModelState.AddModelError("", "This photo length is bigger 5MB");
                return View(updateCategoryVM);
            }
            var newUrl = Guid.NewGuid().ToString() + updateCategoryVM.Photo.FileName;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", newUrl);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                updateCategoryVM.Photo.CopyTo(file);
            }
            existItem.ImageUrl = newUrl;
            existItem.Name = updateCategoryVM.Name;
            existItem.UpdatedTime = DateTime.Now;
            existItem.AddedBy = User.Identity.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
