using Final_Project.Models.Entity;

namespace Final_Project.Models
{
    public class Slider : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}
