using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminCategory;
using Final_Project.ViewModels.AdminWorker;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class WorkerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public WorkerController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1, int take = 5)
        {
            WorkerVM workerVM = new WorkerVM();
            workerVM.Workers = _context.Workers
                .Where(s => !s.IsDeleted)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.Workers.Where(p => !p.IsDeleted).Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<Workers> pagination = new(workerVM.Workers, pageCount, page);

            return View(pagination);
        }

        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var existelemnt = _context.Workers.FirstOrDefault(s => s.Id == id);
            existelemnt.IsDeleted = true;
            existelemnt.DeletedTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            var existElement = _context.Workers
                .FirstOrDefault(x => x.Id == id);
            return View(existElement);
        }
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();
            var existItem = _context.Workers.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();
            UpdateWorkerVM updateWorkerVM = new UpdateWorkerVM();
            updateWorkerVM.FullName = existItem.FullName;
            updateWorkerVM.Position = existItem.Position;
            updateWorkerVM.FacebookUrl = existItem.FacebookUrl;
            updateWorkerVM.InstagramUrl = existItem.InstagramUrl;
            updateWorkerVM.LinkedinUrl = existItem.LinkedinUrl;
            updateWorkerVM.Location = existItem.Location;
            updateWorkerVM.PhoneNumber = existItem.PhoneNumber;
            updateWorkerVM.Email = existItem.Email;
            updateWorkerVM.Salary = existItem.Salary;
            updateWorkerVM.Experience = existItem.Experience;
            return View(updateWorkerVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, UpdateWorkerVM updateWorkerVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existItem = _context.Workers.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();

            if (updateWorkerVM.Image == null)
            {
                ModelState.AddModelError("", "Please enter the slider photo");
                return View(updateWorkerVM);
            }
            if (!updateWorkerVM.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Please choose the image file");
                return View(updateWorkerVM);
            }
            if (updateWorkerVM.Image.Length / 1024 > 5000)
            {
                ModelState.AddModelError("", "This photo length is bigger 5MB");
                return View(updateWorkerVM);
            }
            var newUrl = Guid.NewGuid().ToString() + updateWorkerVM.Image.FileName;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", newUrl);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                updateWorkerVM.Image.CopyTo(file);
            }
            existItem.ImageUrl = newUrl;
            existItem.FullName = updateWorkerVM.FullName;
            existItem.Position = updateWorkerVM.Position;
            existItem.FacebookUrl = updateWorkerVM.FacebookUrl;
            existItem.InstagramUrl = updateWorkerVM.InstagramUrl;
            existItem.LinkedinUrl = updateWorkerVM.LinkedinUrl;
            existItem.Location = updateWorkerVM.Location;
            existItem.PhoneNumber = updateWorkerVM.PhoneNumber;
            existItem.Email = updateWorkerVM.Email;
            existItem.Salary = updateWorkerVM.Salary;
            existItem.Experience = updateWorkerVM.Experience;
            existItem.UpdatedTime = DateTime.Now;
            existItem.AddedBy = User.Identity.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateWorkerVM createWorkerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (!createWorkerVM.Image.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }
            else if (!createWorkerVM.Image.IsLenghSuit(1000))
            {
                ModelState.AddModelError("Image", "Length of file must be smaller than 1kb");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + createWorkerVM.Image.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createWorkerVM.Image.CopyTo(stream);
            }
            Workers worker = new()
            {
                ImageUrl = fileName,
                FullName = createWorkerVM.FullName,
                Position = createWorkerVM.Position,
                LinkedinUrl = createWorkerVM.LinkedinUrl,
                FacebookUrl = createWorkerVM.FacebookUrl,
                InstagramUrl = createWorkerVM.InstagramUrl,
                Salary = createWorkerVM.Salary,
                Location = createWorkerVM.Location,
                Email = createWorkerVM.Email,
                PhoneNumber = createWorkerVM.PhoneNumber,
                Experience = createWorkerVM.Experience,
                CreatedTime = DateTime.Now,
                AddedBy = User.Identity.Name
            };
            _context.Workers.Add(worker);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
