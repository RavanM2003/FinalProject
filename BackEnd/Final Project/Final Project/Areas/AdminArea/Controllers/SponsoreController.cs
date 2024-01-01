using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminBanner;
using Final_Project.ViewModels.AdminSponsore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SponsoreController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public SponsoreController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1, int take = 5)
        {
            SponsoreVM sponsoreVM = new SponsoreVM();
            sponsoreVM.Sponsores = _context.Sponsores
                .Where(s => !s.IsDeleted)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.Sponsores.Where(p => !p.IsDeleted).Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<Sponsore> pagination = new(sponsoreVM.Sponsores, pageCount, page);

            return View(pagination);
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var existelemnt = _context.Sponsores.FirstOrDefault(s => s.Id == id);
            existelemnt.IsDeleted = true;
            existelemnt.DeletedTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            var existElement = _context.Sponsores
                .FirstOrDefault(x => x.Id == id);
            return View(existElement);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateSponsoreVM createSponsoreVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (!createSponsoreVM.Photo.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }
            else if (!createSponsoreVM.Photo.IsLenghSuit(1000))
            {
                ModelState.AddModelError("Image", "Length of file must be smaller than 1kb");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + createSponsoreVM.Photo.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createSponsoreVM.Photo.CopyTo(stream);
            }
            Sponsore sponsore = new()
            {
                ImageUrl = fileName,
                Name = createSponsoreVM.Title,
                AddedBy = User.Identity.Name
            };
            _context.Sponsores.Add(sponsore);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();
            var existItem = _context.Sponsores.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();
            UpdateSponsoreVM updateSponsoreVM = new UpdateSponsoreVM();
            updateSponsoreVM.Title = existItem.Name;
            return View(updateSponsoreVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, UpdateSponsoreVM updateSponsoreVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existItem = _context.Sponsores.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();

            if (updateSponsoreVM.Photo == null)
            {
                ModelState.AddModelError("", "Please enter the slider photo");
                return View(updateSponsoreVM);
            }
            if (!updateSponsoreVM.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Please choose the image file");
                return View(updateSponsoreVM);
            }
            if (updateSponsoreVM.Photo.Length / 1024 > 5000)
            {
                ModelState.AddModelError("", "This photo length is bigger 5MB");
                return View(updateSponsoreVM);
            }
            var newUrl = Guid.NewGuid().ToString() + updateSponsoreVM.Photo.FileName;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", newUrl);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                updateSponsoreVM.Photo.CopyTo(file);
            }
            existItem.ImageUrl = newUrl;
            existItem.Name = updateSponsoreVM.Title;
            existItem.UpdatedTime = DateTime.Now;
            existItem.AddedBy = User.Identity.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
