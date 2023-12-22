using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.Profile;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment = null)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string name)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ChangeData(ProfileVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (!userVM.Image.IsImage())
            {
                ModelState.AddModelError("Image", "only image");
                return View();
            }
            else if (!userVM.Image.IsLenghSuit(1000))
            {
                ModelState.AddModelError("Image", "Length of file must be smaller than 1kb");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + userVM.Image.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                userVM.Image.CopyTo(stream);
            }
            var sistemdekiUser = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            sistemdekiUser.ImageUrl = fileName;
            IdentityResult result =  _userManager.UpdateAsync(sistemdekiUser).Result;
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userVM);
            }
            return RedirectToAction("Index");
        }
    }
}
