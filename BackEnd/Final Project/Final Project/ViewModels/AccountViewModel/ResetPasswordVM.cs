using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModels.AccountViewModel
{
    public class ResetPasswordVM
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmedPassword { get; set; }
    }
}
