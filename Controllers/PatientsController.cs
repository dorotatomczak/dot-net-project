using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClinic.Data;
using WebClinic.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClinic.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PatientsController : Controller
    {
        private ApplicationDbContext db;

        public PatientsController(ApplicationDbContext db/*IStringLocalizer<AccountController> localizer*/)
        {
            this.db = db;
            //this.localizer = localizer;
        }

        [HttpGet]
        public IActionResult AllPatients()
        {
            var model = new PatientsViewModel();
            model.Patients = db.Patients.ToList();
            return View(model);
        }
    }
}
