using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebClinicGUI.Models.Users;
using Microsoft.EntityFrameworkCore;
using WebClinicGUI.Models;
using System.Collections.Generic;
using WebClinicGUI.Services;
using System.Net.Http;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebClinicGUI.Controllers
{
    public class AddAppointmentController : Controller
    {
        private readonly IStringLocalizer<AddAppointmentController> _localizer;
        private readonly INetworkClient _client;

        public AddAppointmentController(IStringLocalizer<AddAppointmentController> localizer, INetworkClient client)
        {
            _localizer = localizer;
            _client = client;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(PhysicianSpecialization specialization, DateTime date, AppointmentType appointmentType)
        {
            var model = new AddAppointmentViewModel
            {
                FreeTerms = new List<Appointment>()
            };

            if (date < DateTime.Now)
            {
                return View(model);
            }

            try
            {
                var builder = new StringBuilder("Appointments/free/?");
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["specialization"] = specialization.ToString();
                query["date"] = date.ToString();
                query["type"] = appointmentType.ToString();
                builder.Append(query.ToString());
                model.FreeTerms = await _client.SendRequestAsync<List<Appointment>>(HttpMethod.Get, builder.ToString());
                return View(model);
            }
            catch (HttpRequestException)
            {
                //show error
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Select(int physicianId, DateTime time, AppointmentType type)
        {
            var userNameIdentifier = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));

            var userId = Convert.ToInt32(userNameIdentifier.Value.ToString());

            var appointment = new Appointment
            {
                PatientId = userId,
                PhysicianId = physicianId,
                Time = time,
                Type = type,
            };

            try
            {
                await _client.SendRequestWithBodyAsync<Appointment>(HttpMethod.Post, "Appointments", appointment);
                return RedirectToAction("Index", "Calendar");
            }
            catch (HttpRequestException)
            {
                //show error
                return View();
            }
        }
    }

}