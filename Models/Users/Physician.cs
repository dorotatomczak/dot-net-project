using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClinic.Models
{
    enum PhysicianSpecialization
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
        PhysicianSpecialization specialization { get; set; }
        //working hours?
    }
}
