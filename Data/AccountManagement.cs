using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClinic.Models.Users;

namespace WebClinic.Data
{
    public class AccountManagement : IAccountManagement
    {
        private readonly ApplicationDbContext context;

        public AccountManagement(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Patient CreatePatient(Patient patient, string password)
        {
            if (IsEmailAvailable(patient.Email))
            {
                patient.Password = Hash(password);
                context.AppUsers.Add(patient);
                context.SaveChanges();
                if (!IsEmailAvailable(patient.Email))
                {
                    return patient;
                }
            }
            return null;
        }

        private bool IsEmailAvailable(string email)
        {
            return context.AppUsers.SingleOrDefault(user => user.Email == email) == null;
        }

        private string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }
}
