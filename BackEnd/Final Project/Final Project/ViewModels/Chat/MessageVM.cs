using Final_Project.Models;

namespace Final_Project.ViewModels.Chat
{
    public class MessageVM
    {
        public List<Message> Messages { get; set; }
        public AppUser User { get; set; }
    }
}
