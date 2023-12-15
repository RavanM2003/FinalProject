using Final_Project.DAL;
using Final_Project.ViewModels.Footer;
using Final_Project.ViewModels.Header;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM footerVM = new();
            footerVM.Setting = _context.Settings.Where(s => !s.IsDeleted).AsNoTracking().ToDictionary(s => s.Key, s => s.Value);
            return View(footerVM);
        }
    }
}
