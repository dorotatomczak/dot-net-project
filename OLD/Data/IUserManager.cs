using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
