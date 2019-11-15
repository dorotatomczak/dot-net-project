using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClinic.Models;
using WebClinic.Models.Users;

namespace WebClinic.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    }
}
