using Microsoft.AspNetCore.Http;
using WebClinic.Models.Users;

namespace WebClinic.Data
{
    public interface IUserManager
    {
        Patient CreatePatient(Patient patient, string password);
        bool SignIn(HttpContext httpContext, string email, string password);
        void SignOut(HttpContext httpContext);
    }
}
