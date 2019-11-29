using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebClinicGUI.Models.Users;
using Microsoft.EntityFrameworkCore;
using WebClinicGUI.Models;
using System.Collections.Generic;

namespace WebClinicGUI.Controllers
{
    public class AddAppointmentController : Controller
    {
        private readonly IStringLocalizer<AddAppointmentController> _localizer;
        //private readonly ApplicationDbContext _context;

        public AddAppointmentController(IStringLocalizer<AddAppointmentController> localizer) //, ApplicationDbContext context)
        {
            _localizer = localizer;
           // _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PhysicianSpecialization specialization, DateTime date, AppointmentType appointmentType)
        {
            var addApointmentVM = new AddAppointmentViewModel
            {
                FreeTerms = new List<PhysicianFreeTerm>()
            };

            if (date < DateTime.Now)
            {
                return View(addApointmentVM);
            }

            //var physicians = _context.Physicians
            //    .Where(p => p.Specialization == specialization)
            //    .ToList();

            //int interval = 1; // 1 hour

            //foreach (var physician in physicians)
            //{
            //    DateTime startSpan = new DateTime(date.Year, date.Month, date.Day, 8, 0, 0);
            //    DateTime endSpan = new DateTime(date.Year, date.Month, date.Day, 16, 0, 0);

            //    while (startSpan < endSpan)
            //    {
            //        var result = physician.Appointments?.FirstOrDefault(a => a.Time == startSpan);
            //        if (result == null)
            //        {
            //            addApointmentVM.FreeTerms.Add(new PhysicianFreeTerm
            //            {
            //                Physician = physician,
            //                FreeTerm = startSpan
            //            });
            //        }
            //        startSpan = startSpan.AddHours(interval);
            //    }
            //}

            return View(addApointmentVM);
        }
    }

}