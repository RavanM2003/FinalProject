namespace Final_Project.ViewModels.AdminBlog
{
    public class CreateBlogVM
    {
        public IFormFile Photo { get; set; }
        public string Topic { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime WriteTime { get; set; }
    }
}
