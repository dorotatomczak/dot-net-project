using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebClinicGUI.Models;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;
using WebClinicGUI.Utils;
using Microsoft.Extensions.Localization;
using WebClinicGUI.Helpers;

namespace WebClinicGUI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Receptionist,Physician")]
    public class PhysiciansController : Controller
    {
        private readonly INetworkClient _client;
        private readonly IStringLocalizer<PhysiciansController> _localizer;
        private readonly ICacheService _cacheService;

        public PhysiciansController(INetworkClient client, ICacheService cacheService, IStringLocalizer<PhysiciansController> localizer)
        {
            _client = client;
            _localizer = localizer;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> AllPhysicians()
        {
            if (TempData["message"] != null)
            {
                ModelState.AddModelError("info", _localizer[TempData["message"].ToString()]);
            }
            try
            {
                var model = new PhysiciansViewModel();
                model.Physicians = await _cacheService.GetPhysiciansAsync();

                if (model.Physicians == null)
                {
                    model.Physicians = await _client.SendRequestAsync<List<Physician>>(HttpMethod.Get, "Physicians");
                    _cacheService.SetPhysiciansAsync(model.Physicians);
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
                var physician = await _client.SendRequestAsync<Physician>(HttpMethod.Get, $"Physicians/{id}");

                if (physician == null)
                {
                    return NotFound();
                }
                var model = PhysicianViewModel.GetModel(physician);
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
                var physician = await _client.SendRequestAsync<Physician>(HttpMethod.Get, $"Physicians/{id}");

                if (physician == null)
                {
                    return NotFound();
                }
                var model = PhysicianViewModel.GetModel(physician);

                TempData["message"] = "Your changes has been saved.";
                return View(model);
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] PhysicianViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var physician = await _client.SendRequestAsync<Physician>(HttpMethod.Get, $"Physicians/{id}");
                    model.UpdatePhysician(ref physician);

                    await _client.SendRequestWithBodyAsync<Physician>(HttpMethod.Put, $"Physicians/{id}", physician);
                    return RedirectToAction(nameof(PhysiciansController.Details), new { id });
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
                var physician = await _client.SendRequestAsync<Physician>(HttpMethod.Delete, $"Physicians/{id}");
                _cacheService.InvalidateCacheAsync();
                TempData["message"] = "Physician has been deleted.";
                return RedirectToAction(nameof(PhysiciansController.AllPhysicians));
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }
        [HttpGet]
        public IActionResult AddPhysician()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhysician([FromForm] PhysicianViewModel model)
        {
            if (ModelState.IsValid)
            {
                var physician = new Physician
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Sex = model.Sex.GetValueOrDefault(),
                    DateOfBirth = model.DateOfBirth.GetValueOrDefault(),
                    Role = Role.Physician,
                    Password = HashUtils.Hash(model.LastName.ToLower()),
                    Specialization = model.Specialization.GetValueOrDefault()
                };

                try
                {
                    await _client.SendRequestWithBodyAsync(HttpMethod.Post, "Account/RegisterPhysician", physician);
                    _cacheService.InvalidateCacheAsync();
                    TempData["message"] = "Physician has been added.";
                    return RedirectToAction("AllPhysicians");
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError("EmailExist", _localizer["Email is already taken"]);
                }
            }
            return View(model);
        }
    }
}
