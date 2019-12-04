using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClinicAPI.Data;
using WebClinicAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebClinicAPI.Models.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebClinicAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointments/free
        // Get free terms
        [HttpGet]
        [Route("free")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetFreeTerms(
            [FromQuery] PhysicianSpecialization specialization,
            [FromQuery] DateTime startDate,
            [FromQuery] AppointmentType type)
        {
            // Get all physicians with the required specialization
            var physicians = await _context.Physicians
                .Where(p => p.Specialization == specialization)
                .ToListAsync();

            // TODO: change this
            // By default, search for free appointments in the 2 days span from the start date
            TimeSpan defaultDateSpan = new TimeSpan(2, 0, 0, 0);
            DateTime endDate = startDate.Add(defaultDateSpan);

            // TODO: allow setting appointment span for each physician separately
            TimeSpan defaultAppointmentTimeSpan = new TimeSpan(0, 30, 0);

            var freeAppointments = new List<Appointment>();

            foreach (var physician in physicians)
            {
                // TODO: allow setting working hours for each physician separately

                // Get physician's appointments sorted by date
                var appointments = await _context.Appointments
                    .Where(a => a.PhysicianId == physician.Id)
                    .Where(a => a.Time >= startDate)
                    .Where(a => a.Time <= endDate)
                    .OrderBy(a => a.Time)
                    .ToListAsync();

                // iterate ovet days in selected timespan
                for (DateTime day = startDate; day < endDate; day = day.AddDays(1))
                {
                    // iterate over potential appointment hours in current day
                    for (DateTime appointmentTime = day.AddHours(8); appointmentTime < day.AddHours(16); appointmentTime += defaultAppointmentTimeSpan)
                    {
                        if (appointments.Count == 0)
                        {
                            // current appointmentTime is free
                            freeAppointments.Add(new Appointment
                            {
                                Physician = physician,
                                PhysicianId = physician.Id,
                                Time = appointmentTime,
                                Type = type
                            });
                        }
                        else
                        {
                            DateTime closestAppointmentTime = appointments.ElementAt(0).Time;
                            if (
                                (appointmentTime < closestAppointmentTime && appointmentTime + defaultAppointmentTimeSpan > closestAppointmentTime) ||
                                (appointmentTime >= closestAppointmentTime))
                            {
                                // current appointmentTime is taken
                                appointments.RemoveAt(0);
                            }
                            else
                            {
                                // current appointmentTime is free
                                freeAppointments.Add(new Appointment
                                {
                                    Physician = physician,
                                    PhysicianId = physician.Id,
                                    Time = appointmentTime,
                                    Type = type
                                });
                            }
                        }
                    }
                }
            }
            return freeAppointments;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments([FromQuery] int physicianId = 0, [FromQuery] int patientId = 0)
        {
            if (HttpContext.User.IsInRole("Receptionist"))
            {
                if (physicianId != 0 && patientId != 0)
                {
                    return await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Physician)
                    .Where(a => a.PhysicianId == physicianId && a.PatientId == patientId)
                    .ToListAsync();
                }
                else if (physicianId != 0)
                {
                    return await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Physician)
                    .Where(a => a.PhysicianId == physicianId)
                    .ToListAsync();
                }
                else if (patientId != 0)
                {
                    return await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Physician)
                    .Where(a => a.PatientId == patientId)
                    .ToListAsync();
                }
                else
                {
                    return await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Physician)
                    .ToListAsync();
                }
            }
            else if (HttpContext.User.IsInRole("Patient") || HttpContext.User.IsInRole("Physician"))
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                if (HttpContext.User.IsInRole("Patient"))
                {
                    return await _context.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Physician)
                        .Where(a => a.PatientId.ToString() == claimsIdentity.Name)
                        .ToListAsync();
                }
                else if (HttpContext.User.IsInRole("Physician"))
                {
                    return await _context.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Physician)
                        .Where(a => a.PhysicianId.ToString() == claimsIdentity.Name)
                        .ToListAsync();
                }
            }
            return Unauthorized();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            if (HttpContext.User.IsInRole("Receptionist") ||
                HttpContext.User.IsInRole("Patient") && appointment.PatientId.ToString() == claimsIdentity.Name ||
                HttpContext.User.IsInRole("Physician") && appointment.PhysicianId.ToString() == claimsIdentity.Name)
            {
                return appointment;
            }
            else
            {
                return Unauthorized();
            }
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            if (HttpContext.User.IsInRole("Receptionist") ||
                HttpContext.User.IsInRole("Patient") && appointment.PatientId.ToString() == claimsIdentity.Name ||
                HttpContext.User.IsInRole("Physician") && appointment.PhysicianId.ToString() == claimsIdentity.Name)
            {
                if (id != appointment.Id)
                {
                    return BadRequest();
                }

                _context.Entry(appointment).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            return Unauthorized();
        }

        // POST: api/Appointments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            if (HttpContext.User.IsInRole("Receptionist") ||
                HttpContext.User.IsInRole("Patient") && appointment.PatientId.ToString() == claimsIdentity.Name ||
                HttpContext.User.IsInRole("Physician") && appointment.PhysicianId.ToString() == claimsIdentity.Name)
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
            }
            else
            {
                return Unauthorized();
            }
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            if (HttpContext.User.IsInRole("Receptionist") ||
                HttpContext.User.IsInRole("Patient") && appointment.PatientId.ToString() == claimsIdentity.Name ||
                HttpContext.User.IsInRole("Physician") && appointment.PhysicianId.ToString() == claimsIdentity.Name)
            {
                if (appointment == null)
                {
                    return NotFound();
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();

                return appointment;
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
