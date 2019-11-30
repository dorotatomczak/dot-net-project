﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WebClinicGUI.Controllers;
using WebClinicGUI.Models.Calendar;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;

namespace WebClinicGUI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CalendarController : Controller
    {
        private readonly IStringLocalizer<CalendarController> _localizer;
        private readonly ILogger<CalendarController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INetworkClient _client;

        public CalendarController(ILogger<CalendarController> logger, IStringLocalizer<CalendarController> localizer, IHttpContextAccessor accessor, INetworkClient client)
        {
            _localizer = localizer;
            _logger = logger;
            _client = client;
            _contextAccessor = accessor;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToViewBasedOnRole();
        }

        private IActionResult RedirectToViewBasedOnRole()
        {
            if (_contextAccessor.HttpContext.User.IsInRole("Receptionist"))
            {
                return RedirectToAction(nameof(CalendarController.Receptionist));
            }
            else if (_contextAccessor.HttpContext.User.IsInRole("Physician"))
            {
                return RedirectToAction(nameof(CalendarController.Physician));
            }
            else if (_contextAccessor.HttpContext.User.IsInRole("Patient"))
            {
                return RedirectToAction(nameof(CalendarController.Patient));
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
        }

        [Authorize(Roles = "Receptionist")]
        public async Task<IActionResult> Receptionist()
        {
            var events = await GetEventsAsync();
            var patients = await GetAllPatientsAsync();
            var physicians = await GetAllPhysiciansAsync();

            AvailableResources resources = new AvailableResources
            { 
                    Patients = patients,
                    Physicians = physicians
            };

            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Resources = resources,
                Events = events
            };

            ViewBag.ViewType = Request.Cookies["ViewType"];
            return View(eventsViewModel);
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Patient()
        {
            var events = await GetEventsAsync();
            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Resources = null,
                Events = events
            };

            ViewBag.ViewType = Request.Cookies["ViewType"];
            return View(eventsViewModel);
        }

        [Authorize(Roles = "Physician")]
        public async Task<IActionResult> Physician()
        {
            var events = await GetEventsAsync();
            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Resources = null,
                Events = events
            };

            ViewBag.ViewType = Request.Cookies["ViewType"];
            return View(eventsViewModel);
        }

        [HttpGet]
        public IActionResult Day()
        {
            SetViewType("Day");
            return RedirectToViewBasedOnRole();
        }

        [HttpGet]
        public IActionResult Week()
        {
            SetViewType("Week");
            return RedirectToViewBasedOnRole();
        }

       [HttpGet]
        public IActionResult Month()
        {
            SetViewType("Month");
            return RedirectToViewBasedOnRole();
        }

        [HttpGet]
        public void SetViewType(string viewType)
        {
            Response.Cookies.Append("ViewType", viewType,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                }
            );
        }

        private async Task<List<Patient>> GetAllPatientsAsync()
        {
            List<Patient> patients;
            try
            {
                patients = await _client.SendRequestAsync<List<Patient>>(HttpMethod.Get, "Patients");
            }
            catch (HttpRequestException)
            {
                // TODO: error
                patients = null;
            }
            return patients;
        }

        private async Task<List<Physician>> GetAllPhysiciansAsync()
        {
            List<Physician> physicians;
            try
            {
                physicians = await _client.SendRequestAsync<List<Physician>>(HttpMethod.Get, "Physicians");
            }
            catch (HttpRequestException)
            {
                // TODO: error
                physicians = null;
            }
            return physicians;
        }

        [HttpPost]
        public async Task<ViewResult> ApplyFilters([FromForm] int physicianId, [FromForm] int patientId)
        {
            // load again
            var events = await GetEventsAsync(physicianId, patientId);
            var patients = await GetAllPatientsAsync();
            var physicians = await GetAllPhysiciansAsync();

            AvailableResources resources = new AvailableResources
            {
                Patients = patients,
                Physicians = physicians
            };

            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Resources = resources,
                Events = events
            };

            return View(nameof(Receptionist), eventsViewModel);
        }

        private async Task<List<CalendarEvent>> GetEventsAsync()
        {
            try {
                return await _client.SendRequestAsync<List<CalendarEvent>>(HttpMethod.Get, "Appointments/Events");
            }
            catch (HttpRequestException)
            {
                // TODO: error
                return null;
            }
        }

        private async Task<List<CalendarEvent>> GetEventsAsync(int physicianId, int patientId)
        {
            try
            {
                return await _client.SendRequestAsync<List<CalendarEvent>>(HttpMethod.Get, "Appointments/Events?physicianId=" + physicianId + "&patientId=" + patientId);
            }
            catch (HttpRequestException)
            {
                // TODO: error
                return null;
            }
        }
    }
}