using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClinicAPI.Data;
using WebClinicAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebClinicAPI.Models.Users;

namespace WebClinicAPI.Controllers
{
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
                DateTime currentDayTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 8, 0, 0); // from 8:00
                DateTime endDayTime = currentDayTime.AddHours(8);

                // Get physician's appointments sorted by date
                var appointments = await _context.Appointments
                    .Where(a => a.PhysicianId == physician.Id)
                    .Where(a => a.Time >= startDate)
                    .OrderBy(a => a.Time)
                    .ToListAsync();

                while (currentDayTime < endDate)
                {
                    while (currentDayTime < endDayTime)
                    {
                        var closestAppointmentTime = appointments.ElementAt(0).Time;
                        if (closestAppointmentTime - currentDayTime >= defaultAppointmentTimeSpan)
                        {
                            freeAppointments.Add(new Appointment
                            {
                                Physician = physician,
                                PhysicianId = physician.Id,
                                Time = currentDayTime,
                                Type = type
                            });
                            currentDayTime = currentDayTime.Add(defaultAppointmentTimeSpan);
                        }
                        else
                        {
                            currentDayTime = closestAppointmentTime + defaultAppointmentTimeSpan;
                            appointments.RemoveAt(0);
                        }
                    }
                    // Go to next day
                    currentDayTime.Add(new TimeSpan(24, 0, 0));
                    endDayTime = currentDayTime.Add(defaultDateSpan);
                }
            }
            return freeAppointments;
        }

        // GET: api/Appointments
        // Get Appointments as CalendarEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments([FromQuery] int physicianId, [FromQuery] int patientId)
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

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
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

        // POST: api/Appointments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
