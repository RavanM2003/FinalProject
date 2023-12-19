using Final_Project.Models.Entity;

namespace Final_Project.Models
{
    public class Blog : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Topic { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime WriteTime { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
