using Final_Project.Models.Entity;

namespace Final_Project.Models
{
    public class HelpQuestion : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
