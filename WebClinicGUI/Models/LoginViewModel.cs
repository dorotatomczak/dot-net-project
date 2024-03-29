﻿using System.ComponentModel.DataAnnotations;

namespace WebClinicGUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The '{0}' field is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
