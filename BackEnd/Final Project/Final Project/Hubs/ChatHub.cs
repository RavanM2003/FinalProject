using Final_Project.DAL;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Final_Project.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public ChatHub(AppDbContext context, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task SendMessage(string toUserId, string message)
        {
            var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name.ToLower());
            string fromUserId = user.Id;
            Message newMessage = new()
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Content = message,
                SendTime = DateTime.Now
            };
            _context.Messages.Add(newMessage);
            _context.SaveChanges();
            await Clients.User(toUserId).SendAsync("ReceiveMessage", toUserId, message);
        }
        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectionId = Context.ConnectionId;
                var result = _userManager.UpdateAsync(user).Result;
                Clients.All.SendAsync("OnConnect", user.Id);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectionId = null;
                var result = _userManager.UpdateAsync(user).Result;
                Clients.All.SendAsync("DisConnect", user.Id);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task Typing(string toUserId)
        {
            var user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name.ToLower());
            string fromUserId = user.Id;
            await Clients.User(toUserId).SendAsync("Typing", fromUserId);
        }
    }
}
