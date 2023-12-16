using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModels.AccountViewModel
{
    public class LoginVM
    {
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
