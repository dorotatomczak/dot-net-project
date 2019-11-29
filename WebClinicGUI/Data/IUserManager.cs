using Microsoft.AspNetCore.Http;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Data
{
    public interface IUserManager
    {
        Patient CreatePatient(Patient patient, string password);
        bool SignIn(HttpContext httpContext, string email, string password);
        void SignOut(HttpContext httpContext);
    }
}
