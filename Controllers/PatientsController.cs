using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data;
using WebClinic.Models;
using WebClinic.Models.Users;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClinic.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    //[Authorize(Roles = "Receptionist,Physician")]
    public class PatientsController : Controller
    {
        private ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext _context/*IStringLocalizer<AccountController> localizer*/)
        {
            this._context = _context;
            //this.localizer = localizer;
        }

        [HttpGet]
        public IActionResult AllPatients()
        {
            var model = new PatientsViewModel();
            model.Patients = _context.Patients.ToList();
            return View(model);
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost("{id}")]
        public IActionResult Edit(int id, [FromForm] Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(PatientsController.Details), new { id });

        }

        [HttpGet("{id}")]
        public IActionResult Delete(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();

            return RedirectToAction(nameof(PatientsController.AllPatients));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
