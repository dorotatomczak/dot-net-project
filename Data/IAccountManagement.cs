using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClinic.Models.Users;

namespace WebClinic.Data
{
    public interface IAccountManagement
    {
        Patient CreatePatient(Patient patient, string password);

    }
}
