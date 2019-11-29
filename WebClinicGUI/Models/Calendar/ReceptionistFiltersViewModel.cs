using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Models.Calendar
{
    public class ReceptionistFiltersViewModel
    {
        public List<Patient> Patients { get; set; }
        public List<Physician> Physicians { get; set; }
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }
    }
}
