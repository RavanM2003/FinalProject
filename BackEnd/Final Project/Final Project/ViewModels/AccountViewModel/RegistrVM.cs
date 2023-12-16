using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModels.AccountViewModel
{
    public class RegistrVM
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        [Phone, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Country { get; set; }

        [EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare("Password")]
        public string Repeatpassword { get; set; }
    }
}
