using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebClinicAPI.Models.Users;

namespace WebClinicAPI.Models
{
    public enum AppointmentType
    {
        Consultation,
        Cure,
        [Display(Name = "Control check")]
        ControlCheck
    }

    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public int? PhysicianId { get; set; }
        [ForeignKey("PhysicianId")]
        public Physician Physician { get; set; }
        public AppointmentType Type { get; set; }
        public DateTime Time { get; set; }
    }
}
