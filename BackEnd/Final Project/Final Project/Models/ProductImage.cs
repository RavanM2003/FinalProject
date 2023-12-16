using Final_Project.Models.Entity;

namespace Final_Project.Models
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
