using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebClinic.Models.Users
{
    public class Patient : AppUser
    {
        [Display(Name = "IllnessHistory")]
        public string IllnessHistory { get; set; }
        [Display(Name = "RecommendedDrugs")]
        public string RecommendedDrugs { get; set; }
        [JsonIgnore]
        public ICollection<Appointment> Appointments { get; set; }
    }
}
