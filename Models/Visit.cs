using System;

namespace WebClinic.Models
{
    public enum VisitType
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
        public VisitType Type { get; set; }
        public DateTime Time { get; set; }
    }
}
