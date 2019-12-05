using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebClinicGUI.Models.Users
{

    public enum PhysicianSpecialization
    {
        [Display(Name = "Internist")]
        Internist,
        [Display(Name = "Laryngologist")]
        Laryngologist,
        [Display(Name = "Radiologist")]
        Radiologist,
        [Display(Name = "Pulmonologist")]
        Pulmonologist,
        [Display(Name = "Surgeon")]
        Surgeon,
        [Display(Name = "Cardiologist")]
        Cardiologist,
        [Display(Name = "Allergist")]
        Allergist,
        [Display(Name = "Psychologist")]
        Psychologist,
        [Display(Name = "Psychiatrist")]
        Psychiatrist
    }

    public class Physician : AppUser
    {
        public PhysicianSpecialization Specialization { get; set; }
        //working hours?
        public ICollection<Appointment> Appointments { get; set; }
        public override Dictionary<string, string> ToRow()
        {
            var dict = base.ToRow();
            dict.Add("Specialization", Specialization.ToString());
            return dict;
        }
    }
}
