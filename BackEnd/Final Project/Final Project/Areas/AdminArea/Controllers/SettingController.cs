using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminSetting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public SettingController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            SettingVM settingVM = new SettingVM();
            settingVM.Settings = _context.Settings
                .Where(s => !s.IsDeleted).AsNoTracking()
                .ToDictionary(s=>s.Key, s => s.Value);
            return View(settingVM);
        }

        public IActionResult Update(string key)
        {

            var existSetting = _context.Settings.FirstOrDefault(p => p.Key == key);
            if (existSetting == null) return NotFound();

            UpdateSettingVM updateSettingVM = new UpdateSettingVM();
            updateSettingVM.Value = existSetting.Value;
            updateSettingVM.Key = existSetting.Key;
            return View(updateSettingVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(string? key, UpdateSettingVM updateSettingVM)
        {
            if (!ModelState.IsValid) return View();
            var existSetting = _context.Settings.FirstOrDefault(p => p.Key == key);
            if (existSetting == null) return NotFound();
            existSetting.Value = updateSettingVM.Value;
            existSetting.Key = updateSettingVM.Key;
            _context.SaveChanges();
            return RedirectToAction("index", "setting");


        }
    }
}
