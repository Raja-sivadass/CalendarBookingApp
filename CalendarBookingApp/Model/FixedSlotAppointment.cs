using System.ComponentModel.DataAnnotations;

namespace CalendarBookingApp.Model
{
    public class FixedSlotAppointment
    {
        [Key]
        public long Id { get; set; }
        public TimeSpan Time { get; set; }
    }
}
