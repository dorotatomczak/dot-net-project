using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClinic.Models.Users
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public string Role { get; set; }
        [NotMapped]
        public string FullName
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
