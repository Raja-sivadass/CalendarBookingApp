using System.ComponentModel.DataAnnotations;

namespace CalendarBookingApp.Model
{
    public class Appointment
    {
        [Key]
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}