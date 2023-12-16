using Final_Project.Models;

namespace Final_Project.ViewModels.Home
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Workers> Workers { get; set; }
        public List<Sponsore> Sponsores { get; set; }
        public List<Product> Product { get; set; }
        public List<Category> Categories { get; set; }
        public Banner Banner { get; set; }
    }
}
