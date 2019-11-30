using System;
using System.ComponentModel.DataAnnotations;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Models
{    public class AppUserViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Sex")]
        public Sex Sex { get; set; }
    }
}
