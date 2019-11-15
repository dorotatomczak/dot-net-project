using System;
using WebClinic.Models.Users;

namespace WebClinic.Models
{
    public enum AppointmentType
    {
        Consultation,
        Cure,
        ControlCheck
    }

    public class Visit
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Physician Physician { get; set; }
        public AppointmentType Type { get; set; }
        public DateTime Time { get; set; }
    }
}
