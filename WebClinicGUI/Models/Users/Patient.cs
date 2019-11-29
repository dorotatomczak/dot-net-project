using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebClinicGUI.Models.Users
{
    public class Patient : AppUser
    {
        public string IllnessHistory { get; set; }
        public string RecommendedDrugs { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
