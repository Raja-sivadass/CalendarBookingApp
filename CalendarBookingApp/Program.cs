using CalendarBookingApp;
using CalendarBookingApp.Commands;
using Microsoft.Extensions.DependencyInjection;
using ICommand = CalendarBookingApp.Commands.ICommand;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();


        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<AppointmentService>();
        services.AddDbContext<AppointmentsDbContext>();
        services.BuildServiceProvider();

        var appointmentService = services.BuildServiceProvider().GetRequiredService<IAppointmentService>();

        var commandParser = new CommandParser();
        commandParser.RegisterCommand("ADD", new AddCommand());
        commandParser.RegisterCommand("DELETE", new DeleteCommand());
        commandParser.RegisterCommand("FIND", new FindCommand());
        commandParser.RegisterCommand("KEEP", new KeepCommand());

        while (true)
        {
            Console.WriteLine("Available Commands:");
            Console.WriteLine("1. ADD DD/MM HH:mm - Add an appointment");
            Console.WriteLine("2. DELETE DD/MM HH:mm - Delete an appointment");
            Console.WriteLine("3. FIND DD/MM - Find free time slots");
            Console.WriteLine("4. KEEP HH:mm - Keep a time slot for any day");
            Console.WriteLine("5. EXIT - Exit the application");
            Console.WriteLine("Enter command:");

            var input = Console.ReadLine()?.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.ToUpper() == "EXIT")
                return;

            string[] commandArgs = input.Split(' ');
            ICommand command = commandParser.ParseCommand(commandArgs[0]);
            if (command != null)
            {
                command.Execute(appointmentService, commandArgs);
            }
            else
            {
                Console.WriteLine("Invalid command. Please try again.");
            }
        }
    }
}


