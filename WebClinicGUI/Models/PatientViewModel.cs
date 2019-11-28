using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebClinic.Models.Users;

namespace WebClinic.Models
{
    public class PatientViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "The '{0}' field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        public string Email { get; set; }

        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "The '{0}' field is required.")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        [Required(ErrorMessage = "The '{0}' field is required.")]
        public string LastName { get; set; }

        [Display(Name = "DateOfBirth")]
        [Required(ErrorMessage = "The '{0}' field is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        public DateTime DateOfBirth { get; set; }

        [EnumDataType(typeof(Sex))]
        [Display(Name = "Sex")]
        [Required(ErrorMessage = "The '{0}' field is required.")]
        public Sex? Sex { get; set; }

        [Display(Name = "IllnessHistory")]
        public string IllnessHistory { get; set; }
        [Display(Name = "RecommendedDrugs")]
        public string RecommendedDrugs { get; set; }

        public void UpdatePatient(ref Patient p)
        {
            p.FirstName = FirstName;
            p.LastName = LastName;
            p.DateOfBirth = DateOfBirth;
            p.Email = Email;
            p.Sex = Sex.GetValueOrDefault();
            p.IllnessHistory = IllnessHistory;
            p.RecommendedDrugs = RecommendedDrugs;
        }
        public static PatientViewModel GetModel(Patient p)
        {
            var model = new PatientViewModel();
            model.Id = p.Id;
            model.FirstName = p.FirstName;
            model.LastName = p.LastName;
            model.DateOfBirth = p.DateOfBirth;
            model.Sex = p.Sex;
            model.Email = p.Email;
            model.IllnessHistory = p.IllnessHistory;
            model.RecommendedDrugs = p.RecommendedDrugs;
            return model;
        }

    }
}
