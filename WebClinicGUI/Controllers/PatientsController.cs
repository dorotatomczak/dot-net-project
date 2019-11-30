using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClinicGUI.Models;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClinicGUI.Controllers
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
            try
            {
                var model = new PatientsViewModel();
                model.Patients = new List<Patient>();

                model.Patients = await _client.SendRequestAsync<List<Patient>>(HttpMethod.Get, "Patients");
                return View(model);
            }
            catch (HttpRequestException)
            {
                //show error
                return View();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var patient = await _client.SendRequestAsync<Patient>(HttpMethod.Get, $"Patients/{id}");

                if (patient == null)
                {
                    return NotFound();
                }
                var model = PatientViewModel.GetModel(patient);
                return View(model);
            }
            catch (HttpRequestException)
            {
                //show error
                return View();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var patient = await _client.SendRequestAsync<Patient>(HttpMethod.Get, $"Patients/{id}");

                if (patient == null)
                {
                    return NotFound();
                }
                var model = PatientViewModel.GetModel(patient);
                return View(model);
            }
            catch (HttpRequestException)
            {
                //show error
                return View();
            }
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var patient = await _client.SendRequestAsync<Patient>(HttpMethod.Get, $"Patients/{id}");
                    model.UpdatePatient(ref patient);

                    await _client.SendRequestWithBodyAsync<Patient>(HttpMethod.Put, $"Patients/{id}", patient);
                    return RedirectToAction(nameof(PatientsController.Details), new { id });
                }
                catch (HttpRequestException)
                {
                    //show error
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var patient = await _client.SendRequestAsync<Patient>(HttpMethod.Delete, $"Patients/{id}");

            return RedirectToAction(nameof(PatientsController.AllPatients));
        }
    }
}
