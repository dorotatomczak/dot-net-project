using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebClinic.Models.Users;

namespace WebClinic.Models
{
    public class PhysicianFreeTerm
    {
        public Physician Physician { get; set; }
        public DateTime FreeTerm { get; set; }
    }

    public class AddAppointmentViewModel
    {
        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Type of specialist")]
        [EnumDataType(typeof(PhysicianSpecialization))]
        public PhysicianSpecialization? Specialization { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Day of visit")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Type of appointment")]
        [EnumDataType(typeof(AppointmentType))]
        public AppointmentType? AppointmentType { get; set; }

        public List<PhysicianFreeTerm> FreeTerms { get; set; }
    }
}
