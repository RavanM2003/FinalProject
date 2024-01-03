using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Message
    {
        public int Id { get; set; }
        [ForeignKey("FromUserId")]
        public string FromUserId { get; set; }
        [ForeignKey("ToUserId")]
        public string ToUserId { get; set; }
        public string Content { get; set; }
        public DateTime SendTime { get; set; }
    }
}
