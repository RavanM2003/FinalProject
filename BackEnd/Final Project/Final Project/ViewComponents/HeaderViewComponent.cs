﻿using Final_Project.DAL;
using Final_Project.ViewModels.Header;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderVM headerVM = new();
            headerVM.Setting = _context.Settings.Where(s=>!s.IsDeleted).AsNoTracking().ToDictionary(s=>s.Key, s=>s.Value);
            return View(headerVM);
        }
    }
}
