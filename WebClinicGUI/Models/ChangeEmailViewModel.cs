using System.ComponentModel.DataAnnotations;

namespace WebClinicGUI.Models
{
    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "The '{0}' field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        [Display(Name = "Confirm new email")]
        [Compare("NewEmail", ErrorMessage = "The email and confirmation email do not match.")]
        public string ConfirmNewEmail { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
