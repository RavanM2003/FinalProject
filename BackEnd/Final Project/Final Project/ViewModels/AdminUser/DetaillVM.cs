using Final_Project.Models;

namespace Final_Project.ViewModels.AdminUser
{
    public class DetaillVM
    {
        public AppUser User { get; set; }
        public List<Sale> Sales { get; set; }
    }
}
