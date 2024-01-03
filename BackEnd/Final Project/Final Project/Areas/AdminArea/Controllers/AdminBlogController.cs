using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminBlog;
using Final_Project.ViewModels.AdminSponsore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class AdminBlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public AdminBlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1, int take = 5)
        {
            BlogVM blogVM = new BlogVM();
            blogVM.Blogs = _context.Blogs
                .Where(s => !s.IsDeleted)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.Blogs.Where(p => !p.IsDeleted).Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<Blog> pagination = new(blogVM.Blogs, pageCount, page);

            return View(pagination);
        }

        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var existelemnt = _context.Blogs.FirstOrDefault(s => s.Id == id);
            existelemnt.IsDeleted = true;
            existelemnt.DeletedTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            var existElement = _context.Blogs
                .FirstOrDefault(x => x.Id == id);
            if (existElement == null) return NotFound();
            return View(existElement);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateBlogVM createBlogVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (!createBlogVM.Photo.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }
            else if (!createBlogVM.Photo.IsLenghSuit(1000))
            {
                ModelState.AddModelError("Image", "Length of file must be smaller than 1kb");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + createBlogVM.Photo.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createBlogVM.Photo.CopyTo(stream);
            }
            Blog blog = new()
            {
                ImageUrl = fileName,
                Title = createBlogVM.Title,
                Topic = createBlogVM.Topic,
                WriteTime = DateTime.Now,
                Description = createBlogVM.Description,
                AddedBy = User.Identity.Name,
                AppUserId = _userManager.GetUserId(User),
                CreatedTime = DateTime.Now
            };
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();
            var existItem = _context.Blogs.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();
            UpdateBlogVM updateBlogVM = new UpdateBlogVM();
            updateBlogVM.Title = existItem.Title;
            updateBlogVM.Topic = existItem.Topic;
            updateBlogVM.Description = existItem.Description;   
            return View(updateBlogVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, UpdateBlogVM updateBlogVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existItem = _context.Blogs.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();

            if (updateBlogVM.Photo == null)
            {
                ModelState.AddModelError("", "Please enter the slider photo");
                return View(updateBlogVM);
            }
            if (!updateBlogVM.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Please choose the image file");
                return View(updateBlogVM);
            }
            if (updateBlogVM.Photo.Length / 1024 > 5000)
            {
                ModelState.AddModelError("", "This photo length is bigger 5MB");
                return View(updateBlogVM);
            }
            var newUrl = Guid.NewGuid().ToString() + updateBlogVM.Photo.FileName;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", newUrl);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                updateBlogVM.Photo.CopyTo(file);
            }
            existItem.ImageUrl = newUrl;
            existItem.Title = updateBlogVM.Title;
            existItem.Topic = updateBlogVM.Topic;
            existItem.Description = updateBlogVM.Description;
            existItem.UpdatedTime = DateTime.Now;
            existItem.AddedBy = User.Identity.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
