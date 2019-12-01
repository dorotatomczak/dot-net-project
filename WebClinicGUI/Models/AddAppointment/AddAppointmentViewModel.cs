using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Models
{
    public class AddAppointmentViewModel
    {
        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Type of specialist")]
        [EnumDataType(typeof(PhysicianSpecialization))]
        public PhysicianSpecialization? Specialization { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Date start")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "The '{0}' field is required.")]
        [Display(Name = "Type of appointment")]
        [EnumDataType(typeof(AppointmentType))]
        public AppointmentType? AppointmentType { get; set; }
        public List<Appointment> FreeTerms { get; set; }
    }
}
