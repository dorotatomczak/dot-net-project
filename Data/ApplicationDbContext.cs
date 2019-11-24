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
                    DateOfBirth = DateTime.Parse("19/11/1970"),
                    Sex = Sex.Male,
                    Specialization = PhysicianSpecialization.Cardiologist,
                    Password = HashUtils.Hash("pass")
                },
                new Physician
                {
                    Id = 2,
                    Email = "physician2@gmail.com",
                    FirstName = "Elena",
                    LastName = "Fisher",
                    DateOfBirth = DateTime.Parse("1/05/1975"),
                    Sex = Sex.Female,
                    Specialization = PhysicianSpecialization.Surgeon,
                    Password = HashUtils.Hash("pass")
                },
                new Physician
                {
                    Id = 3,
                    Email = "physician3@gmail.com",
                    FirstName = "Victor",
                    LastName = "Sullivan",
                    DateOfBirth = DateTime.Parse("30/06/1967"),
                    Sex = Sex.Male,
                    Specialization = PhysicianSpecialization.Psychiatrist,
                    Password = HashUtils.Hash("pass")
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
                    DateOfBirth = DateTime.Parse("3/04/1990"),
                    Sex = Sex.Male,
                    Password = HashUtils.Hash("pass")
                },
                new Receptionist
                {
                    Id = 5,
                    Email = "receptionist2@gmail.com",
                    FirstName = "Penny",
                    LastName = "Hofstadter",
                    DateOfBirth = DateTime.Parse("19/07/1986"),
                    Sex = Sex.Female,
                    Password = HashUtils.Hash("pass")
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
                    DateOfBirth = DateTime.Parse("22/10/1979"),
                    Sex = Sex.Male,
                    Password = HashUtils.Hash("pass")
                });
            #endregion
        }
    }
}
