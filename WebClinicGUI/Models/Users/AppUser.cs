using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebClinicGUI.Models.Users
{
    public enum Sex
    {
        [Display(Name = "Male")]
        Male,
        [Display(Name = "Female")]
        Female,
        [Display(Name = "It's complicated")]
        ItsComplicated
    }

    public class AppUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public string Role { get; set; }

        public string Token { get; set; }

        public string FullName {
            get { return ToString(); }
         }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

        public virtual Dictionary<string, string> ToRow()
        {
            var user = new Dictionary<string, string>();
            user.Add("Id", Id.ToString());
            user.Add("First name", FirstName);
            user.Add("Last name", LastName);
            user.Add("Email", Email);
            user.Add("Sex", Sex.ToString());
            user.Add("Date of birth", DateOfBirth.ToShortDateString());

            return user;
        }
    }
}
