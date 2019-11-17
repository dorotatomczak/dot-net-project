using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebClinic.Models.Users;

namespace WebClinic.Models
{
    public enum AppointmentType
    {
        Consultation,
        Cure,
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
