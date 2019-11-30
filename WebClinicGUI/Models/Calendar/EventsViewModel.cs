using System.Collections.Generic;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Models.Calendar
{
    public class AvailableResources
    {
        public List<Patient> Patients { get; set; }
        public List<Physician> Physicians { get; set; }
    }

    public class EventsViewModel
    {
        public List<CalendarEvent> Events { get; set; }

        public AvailableResources Resources { get; set; }
    }
}
