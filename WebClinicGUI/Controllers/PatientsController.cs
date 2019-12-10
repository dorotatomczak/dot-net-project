using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebClinicGUI.Helpers;
using WebClinicGUI.Models;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;
using WebClinicGUI.Utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClinicGUI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Receptionist,Physician")]
    public class PatientsController : Controller
    {
        private readonly INetworkClient _client;
        private readonly IStringLocalizer<PatientsController> _localizer;
        private readonly ICacheService _cacheService;
        private readonly IXlsService _xlsService;

        public PatientsController(INetworkClient client, ICacheService cacheService, IStringLocalizer<PatientsController> localizer, IXlsService xlsService)
        {
            _client = client;
            _localizer = localizer;
            _cacheService = cacheService;
            _xlsService = xlsService;
        }

        [HttpGet]
        public async Task<IActionResult> AllPatients()
        {
            if (TempData["message"] != null)
            {
                ModelState.AddModelError("info", _localizer[TempData["message"].ToString()]);
            }
            try
            {
                var model = new PatientsViewModel();
                model.Patients = await _cacheService.GetPatientsAsync();

                if (model.Patients == null)
                {
                    model.Patients = await _client.SendRequestAsync<List<Patient>>(HttpMethod.Get, "Patients");
                    _cacheService.SetPatientsAsync(model.Patients);
                }

                return View(model);
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (TempData["message"] != null)
            {
                ModelState.AddModelError("info", _localizer[TempData["message"].ToString()]);
            }
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
                throw new ServerConnectionException();
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
                throw new ServerConnectionException();
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var patient = await _client.SendRequestAsync<Patient>(HttpMethod.Get, $"Patients/{id}");
                    model.UpdatePatient(ref patient);

                    await _client.SendRequestWithBodyAsync<Patient>(HttpMethod.Put, $"Patients/{id}", patient);
                    TempData["message"] = "Your changes has been saved.";
                    return RedirectToAction(nameof(PatientsController.Details), new { id });
                }
                catch (HttpRequestException)
                {
                    throw new ServerConnectionException();
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var patient = await _client.SendRequestAsync<Patient>(HttpMethod.Delete, $"Patients/{id}");
                _cacheService.InvalidateCacheAsync();
                TempData["message"] = "Patient has been deleted.";
                return RedirectToAction(nameof(PatientsController.AllPatients));
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }

        [HttpGet]
        public IActionResult AddPatient()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromForm] PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Sex = model.Sex.GetValueOrDefault(),
                    DateOfBirth = model.DateOfBirth.GetValueOrDefault(),
                    Role = Role.Patient,
                    Password = HashUtils.Hash(model.LastName.ToLower()),
                    IllnessHistory = model.IllnessHistory,
                    RecommendedDrugs = model.RecommendedDrugs
                };

                try
                {
                    await _client.SendRequestWithBodyAsync(HttpMethod.Post, "Account/RegisterPatient", patient);
                    _cacheService.InvalidateCacheAsync();
                    TempData["message"] = "Patient has been added.";
                    return RedirectToAction("AllPatients");
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError("EmailExist", _localizer["Email is already taken"]);
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> GenerateXls()
        {
            try
            {
                var patients = await _client.SendRequestAsync<IEnumerable<Patient>>(HttpMethod.Get, "Patients");
                _cacheService.SetPatientsAsync(patients.ToList());

                return _xlsService.CreateXlsList(patients, "patients");

            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }
    }
}
