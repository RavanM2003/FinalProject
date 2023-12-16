using Microsoft.AspNetCore.Identity;

namespace Final_Project.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public string Country { get; set; }
        public bool IsSubscribed { get; set; }
        public bool isBlocked { get; set; } 
    }
}
