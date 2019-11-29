using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClinicGUI.Models;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClinic.Controllers
{
    [Route("[controller]/[action]")]
    //[Authorize(Roles = "Receptionist,Physician")]
    public class PatientsController : Controller
    {
        private readonly INetworkClient _client;

        public PatientsController(INetworkClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> AllPatients()
        {
            var model = new PatientsViewModel();
            try
            {
                model.Patients = await _client.SendRequestAsync<List<Patient>>(HttpMethod.Get, "Patients");
                return View(model);
            }
            catch (HttpRequestException)
            {
                //show error
                model.Patients = new List<Patient>();
                return View(model);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            //var patient = _context.Patients.Find(id);
            //if (patient == null)
            //{
            //    return NotFound();
            //}
            //var model = PatientViewModel.GetModel(patient);
            //return View(model);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            //var patient = _context.Patients.Find(id);
            //if (patient == null)
            //{
            //    return NotFound();
            //}

            //var model = PatientViewModel.GetModel(patient);
            //return View(model);
            return NotFound();
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [FromForm] PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var patient = _context.Patients.Find(id);
                //if (id != model.Id)
                //{
                //    return BadRequest();
                //}
                //model.UpdatePatient(ref patient);

                //_context.Entry(patient).State = EntityState.Modified;

                //try
                //{
                //    _context.SaveChanges();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!PatientExists(id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction(nameof(PatientsController.Details), new { id });
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Delete(int id)
        {
            //var patient = _context.Patients.Find(id);
            //if (patient == null)
            //{
            //    return NotFound();
            //}

            //_context.Patients.Remove(patient);
            //_context.SaveChanges();

            return RedirectToAction(nameof(PatientsController.AllPatients));
        }

        private bool PatientExists(int id)
        {
            //return _context.Patients.Any(e => e.Id == id);
            return false;
        }
    }
}
