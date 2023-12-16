using Final_Project.Models.Entity;

namespace Final_Project.Models
{
    public class ProductFeatures : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
