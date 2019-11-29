using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WebClinic.Models.Users;
using WebClinicGUI.Services;

namespace WebClinic.Controllers
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

        //[Authorize]
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
            ViewBag.Patients = await GetAllPatientsAsync();
            ViewBag.Physicians = await GetAllPhysiciansAsync();
            ViewBag.ViewType = Request.Cookies["ViewType"];
            return View();
        }

        [Authorize(Roles = "Patient")]
        public IActionResult Patient()
        {
            ViewBag.ViewType = Request.Cookies["ViewType"];
            return View();
        }

        [Authorize(Roles = "Physician")]
        public IActionResult Physician()
        {
            ViewBag.ViewType = Request.Cookies["ViewType"];
            return View();
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
    }
}