using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using WebClinicGUI.Models;
using WebClinicGUI.Services;


namespace WebClinicGUI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Receptionist")]
    public class AppointmentsController : Controller
    {
        private readonly INetworkClient _client;

        public AppointmentsController(INetworkClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var appointment = await _client.SendRequestAsync<Appointment>(HttpMethod.Delete, $"Appointments/{id}");
                return RedirectToAction(nameof(CalendarController.Receptionist), "Calendar");
            }
            catch (HttpRequestException)
            {
                //show error
                return NotFound();
            }
        }
    }
}