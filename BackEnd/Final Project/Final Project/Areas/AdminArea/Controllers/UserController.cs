using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public UserController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index(int page = 1, int take = 5)
        {
            UserVM userVM = new UserVM();
            userVM.Users = _context.Users
                 .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.Users.Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<AppUser> pagination = new(userVM.Users, pageCount, page);

            return View(pagination);
        }

        public IActionResult ChangeStatus(string? id)
        {
            if (id == null) return NotFound();
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            user.isBlocked = !user.isBlocked;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(string id)
        {
            if (id == null) return NotFound();
            var user = _context.Users.FirstOrDefault(s => s.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Detail(string id, DetaillVM detaillVM)
        {
            if (id == null) return NotFound();
            detaillVM.User = _context.Users.FirstOrDefault(u=>u.Id == id);
            if (detaillVM.User == null) return NotFound();
            detaillVM.Sales = _context.Sales.OrderByDescending(s=>s.CreatedAt).Where(s => s.AppUserId == id).ToList();
            return View(detaillVM);
        }
        public IActionResult Update(string? id)
        {
            var existUser = _context.Users.FirstOrDefault(p => p.Id == id);
            if (existUser == null) return NotFound();

            UpdateUserVM updateUserVM = new UpdateUserVM();
            updateUserVM.Fullname = existUser.FullName;
            updateUserVM.Username = existUser.UserName;
            updateUserVM.PhoneNumber = existUser.PhoneNumber;
            updateUserVM.Country = existUser.Country;
            return View(updateUserVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(string? id, UpdateUserVM updateUserVM)
        {
            if (!ModelState.IsValid) return View();
            var existUser = _context.Users.FirstOrDefault(p => p.Id == id);
            if (existUser == null) return NotFound();
            var existUserWithName = _context.Users.Any(p => p.UserName.ToLower() == updateUserVM.Username.ToLower() && p.Id != id);

            if (existUserWithName)
            {
                ModelState.AddModelError("", "This User is exist");
                return View();
            }
            existUser.FullName = updateUserVM.Fullname;
            existUser.UserName = updateUserVM.Username;
            existUser.PhoneNumber = updateUserVM.PhoneNumber;
            existUser.Country = updateUserVM.Country;
            _context.SaveChanges();
            return RedirectToAction("index", "user");


        }
    }
}
