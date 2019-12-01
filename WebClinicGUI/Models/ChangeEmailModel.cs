
namespace WebClinicGUI.Models
{
    public class ChangeEmailModel
    {
        public ChangeEmailModel(string email, string newEmail, string password)
        {
            Email = email;
            Password = password;
            NewEmail = newEmail;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string NewEmail { get; set; }
    }
}
