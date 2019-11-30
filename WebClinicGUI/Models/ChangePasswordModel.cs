using System.ComponentModel.DataAnnotations;

namespace WebClinicGUI.Models
{
    public class ChangePasswordModel
    {
        public ChangePasswordModel(string email, string oldPassword, string newPassword)
        {
            Email = email;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
