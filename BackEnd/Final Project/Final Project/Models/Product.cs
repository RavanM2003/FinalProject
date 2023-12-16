using Final_Project.Models.Entity;

namespace Final_Project.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double? SalePrice { get; set; }
        public string ProductCode { get; set; }
        public string Color { get; set; }
        public string HowToUse { get; set; }
        public int StockCount { get; set; }
        public double Volume { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public List<ProductFeatures> ProductFeatures { get; set; }
    }
}
