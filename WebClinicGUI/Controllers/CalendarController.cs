using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WebClinicGUI.Helpers;
using WebClinicGUI.Models;
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
        private readonly ICacheService _cacheService;

        public CalendarController(ILogger<CalendarController> logger, IStringLocalizer<CalendarController> localizer,
            IHttpContextAccessor accessor, INetworkClient client, ICacheService cacheService)
        {
            _localizer = localizer;
            _logger = logger;
            _client = client;
            _contextAccessor = accessor;
            _cacheService = cacheService;
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
            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Resources = new AvailableResources
                {
                    Patients = await GetAllPatientsAsync(),
                    Physicians = await GetAllPhysiciansAsync()
                },
                Events = await GetEventsAsync(),
                Filters = null
            };

            ViewBag.ViewType = Request.Cookies["ViewType"];
            return View(eventsViewModel);
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Patient()
        {
            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Resources = null,
                Events = await GetEventsAsync(),
                Filters = null
            };

            ViewBag.ViewType = Request.Cookies["ViewType"];
            return View(eventsViewModel);
        }

        [Authorize(Roles = "Physician")]
        public async Task<IActionResult> Physician()
        {
            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Resources = null,
                Events = await GetEventsAsync(),
                Filters = null
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
                patients = await _cacheService.GetPatientsAsync();

                if (patients == null)
                {
                    patients = await _client.SendRequestAsync<List<Patient>>(HttpMethod.Get, "Patients");
                    _cacheService.SetPatientsAsync(patients);
                }
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
            return patients;
        }

        private async Task<List<Physician>> GetAllPhysiciansAsync()
        {
            List<Physician> physicians;
            try
            {
                physicians = await _cacheService.GetPhysiciansAsync();

                if (physicians == null)
                {
                    physicians = await _client.SendRequestAsync<List<Physician>>(HttpMethod.Get, "Physicians");
                    _cacheService.SetPhysiciansAsync(physicians);
                }
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
            return physicians;
        }

        [HttpPost]
        public async Task<ViewResult> ApplyFilters([FromForm] int physicianId, [FromForm] int patientId)
        {
            EventsViewModel eventsViewModel = new EventsViewModel
            {
                Resources = new AvailableResources
                {
                    Patients = await GetAllPatientsAsync(),
                    Physicians = await GetAllPhysiciansAsync()
                },
                Events = await GetEventsAsync(physicianId, patientId),
                Filters = new GlobalFilters()
            };

            if (physicianId != 0)
            {
                eventsViewModel.Filters.PhysiciansFilters
                    .Add(GetPhysicianFullNameById(eventsViewModel.Resources.Physicians, physicianId));
            }
            if (patientId != 0)
            {
                eventsViewModel.Filters.PatientsFilters.
                    Add(GetPatientFullNameById(eventsViewModel.Resources.Patients, patientId));
            }

            return View(nameof(Receptionist), eventsViewModel);
        }

        private async Task<List<Appointment>> GetAppointmentsAsync(int physicianId = 0, int patientId = 0)
        {
            try
            {
                return await _client.SendRequestAsync<List<Appointment>>(HttpMethod.Get, "Appointments?physicianId=" + physicianId + "&patientId=" + patientId);
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }

        private async Task<List<CalendarEvent>> GetEventsAsync(int physicianId = 0, int patientId = 0)
        {
            var appointments = await GetAppointmentsAsync(physicianId, patientId);
            if (appointments != null)
            {
                List<CalendarEvent> calendarEvents = new List<CalendarEvent>();
                foreach (Appointment app in appointments)
                {
                    CalendarEvent calendarEvent = CalendarEvent.FromAppointment(app);
                    calendarEvent.Text = _localizer["Physician"] + ": " + app.Physician.FullName + ", " +
                                         _localizer["Patient"] + ": " + app.Patient.FullName + ", " +
                                         app.Time.ToString();
                    calendarEvents.Add(calendarEvent);
                }
                return calendarEvents;
            }
            return null;
        }

        private string GetPatientFullNameById(List<Patient> patients, int id)
        {
            foreach(var patient in patients)
            {
                if (patient.Id == id)
                {
                    return patient.FullName;
                }
            }
            return null;
        }

        private string GetPhysicianFullNameById(List<Physician> physicians, int id)
        {
            foreach (var physician in physicians)
            {
                if (physician.Id == id)
                {
                    return physician.FullName;
                }
            }
            return null;
        }
    }
}