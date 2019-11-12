using System;

namespace WebClinic.Models
{
    enum VisitType
    {
        Consultation,
        Cure,
        ControlCheck
    }

    public class Visit
    {
        Patient patient { get; set; }
        Physician physician { get; set; }
        VisitType type { get; set; }
        DateTime time { get; set; }
    }
}
