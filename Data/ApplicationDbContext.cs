using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClinic.Models;

namespace WebClinic.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public DbSet<Physician> physicians { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Visit> visits { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    }
}
