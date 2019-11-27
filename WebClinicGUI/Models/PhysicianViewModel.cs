using System;
using System.ComponentModel.DataAnnotations;
using WebClinic.Models.Users;

namespace WebClinic.Models
{
    public class PhysicianViewModel
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

        [Display(Name = "PhysicianSpecialization")]
        [Required(ErrorMessage = "The '{0}' field is required.")]
        public PhysicianSpecialization? Specialization { get; set; }

        public void UpdatePhysician(ref Physician p)
        {
            p.FirstName = FirstName;
            p.LastName = LastName;
            p.DateOfBirth = DateOfBirth;
            p.Email = Email;
            p.Sex = Sex.GetValueOrDefault();
            p.Specialization = Specialization.GetValueOrDefault();
        }
        public static PhysicianViewModel GetModel(Physician p)
        {
            var model = new PhysicianViewModel();
            model.Id = p.Id;
            model.FirstName = p.FirstName;
            model.LastName = p.LastName;
            model.DateOfBirth = p.DateOfBirth;
            model.Sex = p.Sex;
            model.Email = p.Email;
            model.Specialization = p.Specialization;
            return model;
        }

    }
}
