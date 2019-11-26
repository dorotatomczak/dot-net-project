using System;
using System.ComponentModel.DataAnnotations;
using WebClinic.Models.Users;

namespace WebClinic.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Sex")]
        [EnumDataType(typeof(Sex))]
        public Sex? Sex { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
