using Final_Project.Models.Entity;

namespace Final_Project.Models
{
    public class ProductComment : BaseEntity
    {
        public string Message { get; set; }
        public DateTime WriteTime { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
