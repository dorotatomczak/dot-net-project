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
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "DateOfBirth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        public DateTime DateOfBirth { get; set; }

        [EnumDataType(typeof(Sex))]
        [Display(Name = "Sex")]
        public Sex Sex { get; set; }
        public string Role { get; set; }

        [NotMapped]
        public string FullName {
            get { return ToString(); }
         }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
