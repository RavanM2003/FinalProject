using Final_Project.DAL;
using Final_Project.Models;
using Final_Project.ViewModels.Chat;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public ChatController(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Chat()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            List<AppUser> users = _userManager.Users.Where(u => u.Id != currentUser.Id).ToList();
            return View(users);
        }

        public async Task<IActionResult> Messages(string toUserId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            string fromUserId = "";
            string userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName.ToLower());
            if (user != null)
            {
                fromUserId = user.Id;
            }

            var messages = _appDbContext.Messages.Where(m => m.ToUserId == toUserId && m.FromUserId == fromUserId || m.ToUserId == fromUserId && m.FromUserId == toUserId).OrderBy(m => m.SendTime).ToList();
            var toUser = await _userManager.FindByIdAsync(toUserId);
            MessageVM messageVM = new();
            messageVM.User = toUser;
            messageVM.Messages = messages;
            return Ok(messageVM);
        }
    }
}
