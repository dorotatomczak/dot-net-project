using System;
using System.ComponentModel.DataAnnotations;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Models
{
    public class PhysicianAccountViewModel : AppUserViewModel
    {
        [Display(Name = "Specialization")]
        public PhysicianSpecialization Specialization { get; set; }

    }
}
