﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebClinic.Models.Users
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

    public class Physician : AppUser
    {
        public PhysicianSpecialization Specialization { get; set; }
        //working hours?
        [JsonIgnore]
        public ICollection<Appointment> Appointments { get; set; }
    }
}
