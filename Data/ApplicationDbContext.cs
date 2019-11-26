using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClinic.Models;
using WebClinic.Models.Users;
using WebClinic.Utils;

namespace WebClinic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region PhysicianSeed
            modelBuilder.Entity<Physician>().HasData(
                new Physician
                {
                    Id = 1,
                    Email = "physician1@gmail.com",
                    FirstName = "Nathan",
                    LastName = "Drake",
                    DateOfBirth = new DateTime(1970, 11, 19),
                    Sex = Sex.Male,
                    Specialization = PhysicianSpecialization.Cardiologist,
                    Password = HashUtils.Hash("pass"),
                    Role = Role.Physician
                },
                new Physician
                {
                    Id = 2,
                    Email = "physician2@gmail.com",
                    FirstName = "Elena",
                    LastName = "Fisher",
                    DateOfBirth = new DateTime(1975, 5, 1),
                    Sex = Sex.Female,
                    Specialization = PhysicianSpecialization.Surgeon,
                    Password = HashUtils.Hash("pass"),
                    Role = Role.Physician
                },
                new Physician
                {
                    Id = 3,
                    Email = "physician3@gmail.com",
                    FirstName = "Victor",
                    LastName = "Sullivan",
                    DateOfBirth = new DateTime(1967, 6, 30),
                    Sex = Sex.Male,
                    Specialization = PhysicianSpecialization.Psychiatrist,
                    Password = HashUtils.Hash("pass"),
                    Role = Role.Physician
                });
            #endregion

            #region ReceptionistSeed
            modelBuilder.Entity<Receptionist>().HasData(
                new Receptionist
                {
                    Id = 4,
                    Email = "receptionist1@gmail.com",
                    FirstName = "Rajesh",
                    LastName = "Koothrappali",
                    DateOfBirth = new DateTime(1990, 4, 3),
                    Sex = Sex.Male,
                    Password = HashUtils.Hash("pass"),
                    Role = Role.Receptionist
                },
                new Receptionist
                {
                    Id = 5,
                    Email = "receptionist2@gmail.com",
                    FirstName = "Penny",
                    LastName = "Hofstadter",
                    DateOfBirth = new DateTime(1986, 7, 19),
                    Sex = Sex.Female,
                    Password = HashUtils.Hash("pass"),
                    Role = Role.Receptionist
                });
            #endregion

            #region PatientSeed
            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    Id = 6,
                    Email = "patient1@gmail.com",
                    FirstName = "Geralt",
                    LastName = "Z Rivii",
                    DateOfBirth = new DateTime(1979, 10, 22),
                    Sex = Sex.Male,
                    Password = HashUtils.Hash("pass"),
                    Role = Role.Patient
                });
            #endregion
        }
    }
}
