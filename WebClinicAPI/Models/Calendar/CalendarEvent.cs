using System;

namespace WebClinicAPI.Models.Calendar
{
    public class CalendarEvent
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }

        public static readonly TimeSpan DefaultEventTimeSpan = new TimeSpan(0, 30, 0);
    }
}
