using Final_Project.Models;

namespace Final_Project.ViewModels.Profile
{
    public class ProfileVM
    {
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public IFormFile Image { get; set; }
    }
}
