using System;
using System.ComponentModel.DataAnnotations;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Models
{
    public class PatientAccountViewModel : AppUserViewModel
    {
        [Display(Name = "Recommended drugs")]
        public string RecommendedDrugs { get; set; }

    }
}
