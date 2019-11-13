using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClinic.Models
{
    public enum PhysicianSpecialization
    {
        Internist,
        Laryngologist,
        Radiologist,
        Pulmonologist,
        Surgeon,
        Cardiologist,
        Allergist,
        Psychologist,
        Psychiatrist
    }

    public class Physician : Person
    {
        public PhysicianSpecialization Specialization { get; set; }
        //working hours?
    }
}
