using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebClinic.Models.Users
{
    public class Patient : AppUser
    {
        public string IllnessHistory { get; set; }
        public string RecommendedDrugs { get; set; }
        [JsonIgnore]
        public ICollection<Appointment> Appointments { get; set; }
    }
}
