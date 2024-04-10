using CalendarBookingApp.Model;
using Microsoft.EntityFrameworkCore;

namespace CalendarBookingApp
{
    public interface IAppointmentService
    {
        Task AddAppointmentAsync(DateTime input);
        Task DeleteAppointmentAsync(DateTime input);
        Task FindFreeTimeSlotsAsync(DateTime input);
        Task KeepTimeSlotForAnyDayAsync(TimeSpan parsedDateTime);
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentsDbContext _dbContext;
        public AppointmentService(AppointmentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAppointmentAsync(DateTime inputDate)
        {
            if (IsSlotAvailable(inputDate))
            {
                _dbContext.Appointments.Add(new Appointment { DateTime = inputDate });
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Appointment added successfully.");
            }
        }

        public async Task DeleteAppointmentAsync(DateTime inputDate)
        {
            var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(a => a.DateTime == inputDate);
            if (appointment != null)
            {
                _dbContext.Appointments.Remove(appointment);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Appointment deleted successfully.");
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }

        public async Task FindFreeTimeSlotsAsync(DateTime giveDate)
        {
            var allSlots = GetTimeSlots(giveDate);
            var availableSlots = allSlots.Where(y => IsSlotAvailable(y));
            Console.WriteLine("Free Time Slots:");
            foreach (var slot in availableSlots)
            {
                Console.WriteLine(slot.ToString("dd/MM/yyyy HH:mm"));
            }
        }

        public async Task KeepTimeSlotForAnyDayAsync(TimeSpan parsedTime)
        {
            var isExists = await _dbContext.FixedSlotAppointments.AnyAsync(a => a.Time == parsedTime);
            if (isExists)
            {
                Console.WriteLine("The given time already exists.");
                return;
            }

            _dbContext.FixedSlotAppointments.Add(new FixedSlotAppointment { Time = parsedTime });
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("The slot is added and will be kept for any day.");
        }

        private bool IsSlotAvailable(DateTime givenDateTime)
        {
            // Check if the given DateTime falls within 9 AM to 5 PM
            if (givenDateTime.TimeOfDay < TimeSpan.FromHours(9) || givenDateTime.TimeOfDay >= TimeSpan.FromHours(17))
            {
                Console.WriteLine("Appointment time should be between 9 AM and 5 PM.");
                return false;
            }

            // Check overlap with fixed slot KEEP
            // TO-DO: Implement logic

            // Check if the given DateTime is within the forbidden time range
            if (IsForbiddenTime(givenDateTime))
            {
                Console.WriteLine("Appointments between 4 PM and 5 PM on each second day of the third week of any month are reserved and unavailable.");
                return false; // Exit the program or take appropriate action
            }

            var isOverlap = IsOverlap(givenDateTime);

            if (isOverlap)
            {
                Console.WriteLine("An appointment already exists at that time or overlaps with an existing appointment.");
            }

            return !isOverlap;
        }

        private List<DateTime> GetTimeSlots(DateTime date)
        {
            var startTime = date.Date.AddHours(9); // 9 AM
            var endTime = date.Date.AddHours(17); // 5 PM
            var timeSlots = new List<DateTime>();
            while (startTime < endTime)
            {
                timeSlots.Add(startTime);
                startTime = startTime.AddMinutes(30); // Increment by 30 minutes
            }
            return timeSlots;
        }

        private bool IsOverlap(DateTime givenDateTime)
        {
            // Check if any existing appointment overlaps with the given DateTime
            bool isOverlap = _dbContext.Appointments.Any(a =>
                                a.DateTime.Date == givenDateTime.Date &&
                                givenDateTime >= a.DateTime &&
                                givenDateTime < a.DateTime.AddMinutes(30)); // Assuming appointments are for 30 minutes
            return isOverlap;
        }

        static bool IsForbiddenTime(DateTime dateTime)
        {
            // Check if the given DateTime is in the third week of the month
            if (dateTime.Day >= 15 && dateTime.Day <= 21)
            {
                if (dateTime.DayOfWeek == DayOfWeek.Monday && dateTime.Hour >= 16 && dateTime.Hour < 17)
                {
                    return true; // The time slot is forbidden
                }
            }

            return false; // The time slot is not forbidden
        }
    }
}
