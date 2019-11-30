using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebClinicGUI.Models;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;

namespace WebClinicGUI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Receptionist,Physician")]
    public class PhysiciansController : Controller
    {
        private readonly INetworkClient _client;

        public PhysiciansController(INetworkClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> AllPhysicians()
        {
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
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var physician = await _client.SendRequestAsync<Physician>(HttpMethod.Delete, $"Physicians/{id}");
                return RedirectToAction(nameof(PhysiciansController.AllPhysicians));
            }
            catch (HttpRequestException)
            {
                //show error
                return View();
            }
        }
    }
}
