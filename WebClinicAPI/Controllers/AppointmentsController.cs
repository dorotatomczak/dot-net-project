using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClinicAPI.Data;
using WebClinicAPI.Models;
using WebClinicAPI.Models.Calendar;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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


        // GET: api/Appointments/Events
        // Get Appointments as CalendarEvents
        [HttpGet]
        [Route("events")]
        public ICollection<CalendarEvent> GetEvents([FromQuery] int physicianId, [FromQuery] int patientId)
        {
            List<Appointment> appointments;
            if (physicianId != 0 && patientId != 0)
            {
                appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physician)
                .Where(a => a.PhysicianId == physicianId && a.PatientId == patientId)
                .ToList();
            }
            else if (physicianId != 0)
            {
                appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physician)
                .Where(a => a.PhysicianId == physicianId)
                .ToList();
            }
            else if (patientId != 0)
            {
                appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physician)
                .Where(a => a.PatientId == patientId)
                .ToList();
            }
            else
            {
                appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physician)
                .ToList();
            }

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
