using System.ComponentModel.DataAnnotations;

namespace ISP.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Account login")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Account password")]
        public string Password { get; set; }
    }
}