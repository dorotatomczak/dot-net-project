﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebClinicGUI.Models;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;
using WebClinicGUI.Utils;
using Microsoft.Extensions.Localization;

namespace WebClinicGUI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Receptionist,Physician")]
    public class PhysiciansController : Controller
    {
        private readonly INetworkClient _client;
        private readonly IStringLocalizer<PhysiciansController> _localizer;

        public PhysiciansController(INetworkClient client, IStringLocalizer<PhysiciansController> localizer)
        {
            _client = client;
            _localizer = localizer;
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
                model.Physicians = new List<Physician>();

                model.Physicians = await _client.SendRequestAsync<List<Physician>>(HttpMethod.Get, "Physicians");
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
                //show error
                return View();
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
                //show error
                return View();
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var physician = await _client.SendRequestAsync<Physician>(HttpMethod.Delete, $"Physicians/{id}");
                TempData["message"] = "Physician has been deleted.";
                return RedirectToAction(nameof(PhysiciansController.AllPhysicians));
            }
            catch (HttpRequestException)
            {
                //show error
                return View();
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
