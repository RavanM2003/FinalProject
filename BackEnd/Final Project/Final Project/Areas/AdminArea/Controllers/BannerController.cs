using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminBanner;
using Final_Project.ViewModels.AdminCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public BannerController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1, int take = 5)
        {
            BannerVM bannerVM = new BannerVM();
            bannerVM.Bans = _context.Banners
                .Where(s => !s.IsDeleted)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.Banners.Where(p => !p.IsDeleted).Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<Banner> pagination = new(bannerVM.Bans, pageCount, page);

            return View(pagination);
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var existelemnt = _context.Banners.FirstOrDefault(s => s.Id == id);
            existelemnt.IsDeleted = true;
            existelemnt.DeletedTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            var existElement = _context.Banners
                .FirstOrDefault(x => x.Id == id);
            return View(existElement);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateBannerVM createBannerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (!createBannerVM.Photo.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }
            else if (!createBannerVM.Photo.IsLenghSuit(1000))
            {
                ModelState.AddModelError("Image", "Length of file must be smaller than 1kb");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + createBannerVM.Photo.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createBannerVM.Photo.CopyTo(stream);
            }
            Banner banner = new()
            {
                ImageUrl = fileName,
                Title = createBannerVM.Title,
                AddedBy = User.Identity.Name
            };
            _context.Banners.Add(banner);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();
            var existItem = _context.Banners.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();
            UpdateBannerVM updateBannerVM = new UpdateBannerVM();
            updateBannerVM.Title = existItem.Title;
            return View(updateBannerVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, UpdateBannerVM updateBannerVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existItem = _context.Banners.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();

            if (updateBannerVM.Photo == null)
            {
                ModelState.AddModelError("", "Please enter the slider photo");
                return View(updateBannerVM);
            }
            if (!updateBannerVM.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Please choose the image file");
                return View(updateBannerVM);
            }
            if (updateBannerVM.Photo.Length / 1024 > 5000)
            {
                ModelState.AddModelError("", "This photo length is bigger 5MB");
                return View(updateBannerVM);
            }
            var newUrl = Guid.NewGuid().ToString() + updateBannerVM.Photo.FileName;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", newUrl);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                updateBannerVM.Photo.CopyTo(file);
            }
            existItem.ImageUrl = newUrl;
            existItem.Title = updateBannerVM.Title;
            existItem.UpdatedTime = DateTime.Now;
            existItem.AddedBy = User.Identity.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
