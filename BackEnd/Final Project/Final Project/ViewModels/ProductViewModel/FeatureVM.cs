using Final_Project.Models;

namespace Final_Project.ViewModels.ProductViewModel
{
    public class FeatureVM
    { 
        public string Name { get; set; }
        public string Value { get; set; }
        public List<ProductFeatures?> Features { get; set; }
    }
}
