using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClinicAPI.Data;
using WebClinicAPI.Models.Users;
using WebClinicAPI.Utils;

namespace WebClinicAPI.Services
{

    public interface IUserService
    {
        Patient Create(Patient patient, string password);
        AppUser Authenticate(string email, string password);
        IEnumerable<AppUser> GetAll();
        AppUser GetById(int id);
        Task<AppUser> Update(string email, string oldPassword, string newPassword);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public AppUser Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.AppUsers.SingleOrDefault(x => x.Email == email);

            if (user == null || user.Password != password)
                return null;

            return user;
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _context.AppUsers;
        }

        public AppUser GetById(int id)
        {
            return _context.AppUsers.Find(id);
        }

        public Patient Create(Patient patient, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.AppUsers.Any(x => x.Email == patient.Email))
                throw new AppException("Email \"" + patient.Email + "\" is already taken");


            patient.Password = password;
            _context.AppUsers.Add(patient);
            _context.SaveChanges();

            return patient;
        }
        public async Task<AppUser> Update(string email, string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new AppException("Email is required");
            if (string.IsNullOrWhiteSpace(oldPassword))
                throw new AppException("Old password is required");
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new AppException("New password is required");

            var user = Authenticate(email, oldPassword);

            if (user == null)
                throw new AppException("Password is incorrect");

            user.Password = newPassword;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return user;
        }
    }
}
