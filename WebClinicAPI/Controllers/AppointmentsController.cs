using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClinicAPI.Data;
using WebClinicAPI.Models;
using WebClinicAPI.Models.Calendar;
using Microsoft.EntityFrameworkCore;
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


        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physician)
                .ToListAsync();
        }

        // GET: api/Appointments/free
        // Get free terms
        [HttpGet]
        [Route("free")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetFreeTerms(
            [FromQuery] PhysicianSpecialization specialization,
            [FromQuery] DateTime date,
            [FromQuery] AppointmentType type)
        {
            var physicians = await _context.Physicians
                .Where(p => p.Specialization == specialization)
                .ToListAsync();

            int interval = 1; // 1 hour
            var freeAppointments = new List<Appointment>();

            foreach (var physician in physicians)
            {
                DateTime startSpan = new DateTime(date.Year, date.Month, date.Day, 8, 0, 0); // from 8:00
                DateTime endSpan = new DateTime(date.Year, date.Month, date.Day, 16, 0, 0); // to 16:00

                var appointments = await _context.Appointments
                    .Where(a => a.PhysicianId == physician.Id)
                    .ToListAsync();

                while (startSpan < endSpan)
                {
                    var result = appointments?.FirstOrDefault(a => a.Time == startSpan);
                    if (result == null)
                    {
                        freeAppointments.Add(new Appointment
                        {
                            Physician = physician,
                            PhysicianId = physician.Id,
                            Time = startSpan,
                            Type = type
                        });
                    }
                    startSpan = startSpan.AddHours(interval);
                }
            }

            return freeAppointments;
        }

        // GET: api/Appointments/Events
        // Get Appointments as CalendarEvents
        [HttpGet]
        [Route("events")]
        public ICollection<CalendarEvent> GetEvents([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physician)
                .Where(a => a.Time >= start && a.Time <= end)
                .ToList();
            ICollection<CalendarEvent> events = new HashSet<CalendarEvent>();
            foreach (var appointment in appointments)
            {
                events.Add(new CalendarEvent
                {
                    Start = appointment.Time,
                    End = appointment.Time + CalendarEvent.DefaultEventTimeSpan,
                    Text = "Doctor: " + appointment.Physician.ToString() + ", " +
                           "Patient: " + appointment.Patient.ToString() + ", " +
                           "Time: " + appointment.Time.ToString()
                });
            }
            return events;
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
