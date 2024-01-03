using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminSlider;
using Final_Project.ViewModels.ProductViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index(int page = 1, int take = 5)
        {
            SliderVM sliderVM = new SliderVM();
            sliderVM.Sliders = _context.Sliders
                .Where(s=>!s.IsDeleted)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.Sliders.Where(p => !p.IsDeleted).Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<Slider> pagination = new(sliderVM.Sliders, pageCount, page);

            return View(pagination);
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var existelemnt = _context.Sliders.FirstOrDefault(s => s.Id == id);
            existelemnt.IsDeleted = true;
            existelemnt.DeletedTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id) 
        {
            if(id==null) return NotFound();
            var existElement = _context.Sliders.FirstOrDefault(x=>x.Id == id);
            return View(existElement);
        }
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();
            var existItem = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();
            UpdateSliderVM updateSliderVM = new UpdateSliderVM();
            updateSliderVM.Title = existItem.Title;
            updateSliderVM.Description = existItem.Description;
            updateSliderVM.Link = existItem.Link;
            return View(updateSliderVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, UpdateSliderVM updateSliderVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existItem = _context.Sliders.FirstOrDefault(x=>x.Id == id);
            if (existItem == null) return NotFound();

            if (updateSliderVM.Photo == null)
            {
                ModelState.AddModelError("", "Please enter the slider photo");
                return View(updateSliderVM);
            }
            if (!updateSliderVM.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Please choose the image file");
                return View(updateSliderVM);
            }
            if (updateSliderVM.Photo.Length / 1024 > 5000)
            {
                ModelState.AddModelError("", "This photo length is bigger 5MB");
                return View(updateSliderVM);
            }
            var newUrl = Guid.NewGuid().ToString() + updateSliderVM.Photo.FileName;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", newUrl);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                updateSliderVM.Photo.CopyTo(file);
            }
            existItem.ImageUrl = newUrl;
            existItem.Title = updateSliderVM.Title;
            existItem.Description = updateSliderVM.Description;
            existItem.Link = existItem.Link;
            existItem.UpdatedTime = DateTime.Now;
            existItem.AddedBy = User.Identity.Name;
            _context.SaveChanges();
            return RedirectToAction("index", "slider");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateSliderVM createSliderVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (!createSliderVM.Image.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }
            else if (!createSliderVM.Image.IsLenghSuit(1000))
            {
                ModelState.AddModelError("Image", "Length of file must be smaller than 1kb");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + createSliderVM.Image.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createSliderVM.Image.CopyTo(stream);
            }
            Slider slider = new()
            {
                ImageUrl = fileName,
                Title = createSliderVM.Title,
                Description = createSliderVM.Description,
                Link = createSliderVM.Link,
                AddedBy = User.Identity.Name
            };
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("Index", "Slider");
        }
    }
}
