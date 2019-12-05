using System.Collections.Generic;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Models.Calendar
{
    public class AvailableResources
    {
        public List<Patient> Patients { get; set; }
        public List<Physician> Physicians { get; set; }
    }
    public class GlobalFilters
    {
        public HashSet<string> PatientsFilters { get; set; }
        public HashSet<string> PhysiciansFilters { get; set; }

        public GlobalFilters()
        {
            PatientsFilters = new HashSet<string>();
            PhysiciansFilters = new HashSet<string>();
        }
    }

    public class EventsViewModel
    {
        public List<CalendarEvent> Events { get; set; }

        public AvailableResources Resources { get; set; }

        public GlobalFilters Filters { get; set; }
    }
}
