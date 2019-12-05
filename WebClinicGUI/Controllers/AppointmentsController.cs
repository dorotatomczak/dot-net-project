using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebClinicGUI.Helpers;
using WebClinicGUI.Models;
using WebClinicGUI.Services;


namespace WebClinicGUI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Receptionist")]
    public class AppointmentsController : Controller
    {
        private readonly IStringLocalizer<AddAppointmentController> _localizer;
        private readonly INetworkClient _client;

        public AppointmentsController(IStringLocalizer<AddAppointmentController> localizer, INetworkClient client)
        {
            _localizer = localizer;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var appointment = await _client.SendRequestAsync<Appointment>(HttpMethod.Delete, $"Appointments/{id}");
                TempData["Message"] = "AppointmentCanceled";
                return RedirectToAction(nameof(CalendarController.Receptionist), "Calendar");
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }
    }
}