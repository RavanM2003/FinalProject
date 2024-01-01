using Final_Project.Models;

namespace Final_Project.ViewModels.ProductViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public Category Category { get; set; }
    }
}
