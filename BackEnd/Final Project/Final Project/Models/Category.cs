using Final_Project.Models.Entity;

namespace Final_Project.Models
{
    public class Category : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
