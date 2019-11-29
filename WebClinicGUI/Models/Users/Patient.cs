using System.Collections.Generic;

namespace WebClinicGUI.Models.Users
{
    public class Patient : AppUser
    {
        public string IllnessHistory { get; set; }
        public string RecommendedDrugs { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
