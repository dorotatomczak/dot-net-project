using System.Collections.Generic;
using System.Linq;
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

            if (user == null || user.Password != HashUtils.Hash(password))
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


            patient.Password = HashUtils.Hash(password);
            _context.AppUsers.Add(patient);
            _context.SaveChanges();

            return patient;
            //return _context.Patients
            //    .Where(u => u.Email == patient.Email && u.Password == patient.Password)
            //    .SingleOrDefault();
        }
    }
}
