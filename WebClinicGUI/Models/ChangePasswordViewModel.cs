using System.ComponentModel.DataAnnotations;

namespace WebClinicGUI.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "The '{0}' field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
