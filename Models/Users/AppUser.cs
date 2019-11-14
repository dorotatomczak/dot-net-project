using System;
using System.ComponentModel.DataAnnotations;

namespace WebClinic.Models.Users
{

    public enum Sex
    {
        Male,
        Female,
        ItsComplicated
    }

    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
    }
}
