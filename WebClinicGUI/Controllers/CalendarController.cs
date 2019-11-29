using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WebClinic.Models.Users;

namespace WebClinic.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CalendarController : Controller
    {
        private readonly IStringLocalizer<CalendarController> _localizer;
        private readonly ILogger<CalendarController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public CalendarController(ILogger<CalendarController> logger, IStringLocalizer<CalendarController> localizer, IHttpContextAccessor accessor)
        {
            _localizer = localizer;
            _logger = logger;
            _httpClient = new HttpClient();
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
                return RedirectToAction("Receptionist");
            }
            else if (_contextAccessor.HttpContext.User.IsInRole("Physician"))
            {
                return RedirectToAction("Physician");
            }
            else if (_contextAccessor.HttpContext.User.IsInRole("Patient"))
            {
                return RedirectToAction("Patient");
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public IActionResult Receptionist()
        {
            ViewBag.Patients = GetAllPatients();
            ViewBag.Physicians = GetAllPhysicians();
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

        private HttpResponseMessage ApiCallGet(string Url)
        {
            return _httpClient.GetAsync(Url).Result;
        }

        private List<Patient> GetAllPatients()
        {
            var response = ApiCallGet("http://localhost:56174/api/patients");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize(result, typeof(List<Patient>), options) as List<Patient>;
            }
            else
            {
                return null;    // TODO: error
            }
        }

        private List<Physician> GetAllPhysicians()
        {
            var response = ApiCallGet("http://localhost:56174/api/physicians");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize(result, typeof(List<Physician>), options) as List<Physician>;
            }
            else
            {
                return null;    // TODO: error
            }
        }
    }
}