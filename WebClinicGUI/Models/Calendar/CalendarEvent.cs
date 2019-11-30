using System;

namespace WebClinicGUI.Models.Calendar
{
    public class CalendarEvent
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public Appointment Appointment { get; set; }

        public static readonly TimeSpan DefaultEventTimeSpan = new TimeSpan(0, 30, 0);
    
        public static CalendarEvent FromAppointment(Appointment app)
        {
            return new CalendarEvent
            {
                Start = app.Time,
                End = app.Time + CalendarEvent.DefaultEventTimeSpan,
                Text = "Physician " + app.Physician.FullName + ", Patient " + app.Patient.FullName,
                Appointment = app
            };
        }
    
    }
}
