using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using WebClinicGUI.Models.Users;

namespace WebClinicGUI.Data
{
    public class UserManager : IUserManager
    {
       // private readonly ApplicationDbContext context;

        public UserManager()//ApplicationDbContext context)
        {
           // this.context = context;
        }

        public bool SignIn(HttpContext httpContext, string email, string password)
        {
          //  var dbUserData = context.AppUsers.Where(u => u.Email == email && u.Password == HashUtils.Hash(password))
          //      .SingleOrDefault();

            //if (dbUserData != null)
            //{
            //    ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(dbUserData), CookieAuthenticationDefaults.AuthenticationScheme);
            //    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            //    httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //    return true;
            //}
            return false;
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(AppUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            return claims;
        }

        public Patient CreatePatient(Patient patient, string password)
        {
            //if (IsEmailAvailable(patient.Email))
            //{
            //    patient.Password = HashUtils.Hash(password);
            //    context.AppUsers.Add(patient);
            //    context.SaveChanges();
            //    return context.Patients.Where(u => u.Email == patient.Email && u.Password == patient.Password)
            //    .SingleOrDefault();
            //}
            return null;
        }

        private bool IsEmailAvailable(string email)
        {
            //  return context.AppUsers.SingleOrDefault(user => user.Email == email) == null;
            return false;
        }
    }
}
