namespace Final_Project.ViewModels.ProductViewModel
{
    public class UpdateProductVM
    {
        public List<IFormFile> Photos { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double? SalePrice { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string HowToUse { get; set; }
        public int StockCount { get; set; }
        public double Volume { get; set; }
        public int CategoryId { get; set; }
    }
}
