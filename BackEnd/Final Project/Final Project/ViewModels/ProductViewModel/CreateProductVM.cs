using Final_Project.Models;

namespace Final_Project.ViewModels.ProductViewModel
{
    public class CreateProductVM
    {
        public List<IFormFile> Images { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double? SalePrice { get; set; }
        public string ProductCode { get; set; }
        public string Color { get; set; }
        public string HowToUse { get; set; }
        public int StockCount { get; set; }
        public double Volume { get; set; }
        public int CategoryId { get; set; }
    }
}
