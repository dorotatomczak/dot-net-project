using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinicGUI.Models.Users
{
    public enum Sex
    {
        Male,
        Female,
        ItsComplicated
    }

    public class AppUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public string Role { get; set; }

        public string FullName {
            get { return ToString(); }
         }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
