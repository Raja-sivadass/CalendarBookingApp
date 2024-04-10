using CalendarBookingApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CalendarBookingApp
{
    public class AppointmentsDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<FixedSlotAppointment> FixedSlotAppointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("AppointmentsDbConnection")!;

                optionsBuilder.UseSqlServer(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
