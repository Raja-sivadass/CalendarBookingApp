using System.Globalization;

namespace CalendarBookingApp.Commands
{

    public interface ICommand
    {
        void Execute(IAppointmentService appointmentService, string[] commandArgs);
    }

    public class CommandParser
    {
        private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public void RegisterCommand(string commandName, ICommand command)
        {
            commands[commandName.ToUpper()] = command;
        }

        public ICommand ParseCommand(string commandName)
        {
            if (commands.TryGetValue(commandName.ToUpper(), out ICommand command))
            {
                return command;
            }
            return null;
        }
    }


    public class AddCommand : ICommand
    {
        public void Execute(IAppointmentService appointmentService, string[] commandArgs)
        {
            if (commandArgs.Length < 3)
            {
                Console.WriteLine("Usage: ADD DD/MM HH:mm");
                return;
            }

            if (DateTime.TryParseExact(commandArgs[1] + " " + commandArgs[2], "dd/MM HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                appointmentService.AddAppointmentAsync(parsedDateTime);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
    }

    public class DeleteCommand : ICommand
    {
        public void Execute(IAppointmentService appointmentService, string[] commandArgs)
        {
            if (commandArgs.Length < 3)
            {
                Console.WriteLine("Usage: DELETE DD/MM HH:mm");
                return;
            }

            if (DateTime.TryParseExact(commandArgs[1] + " " + commandArgs[2], "dd/MM HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
            {
                appointmentService.DeleteAppointmentAsync(parsedDateTime);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
    }

    public class FindCommand : ICommand
    {
        public void Execute(IAppointmentService appointmentService, string[] commandArgs)
        {
            if (commandArgs.Length < 2)
            {
                Console.WriteLine("Usage: FIND DD/MM");
                return;
            }

            if (DateTime.TryParseExact(commandArgs[1], "dd/MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                appointmentService.FindFreeTimeSlotsAsync(parsedDate);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
    }

    public class KeepCommand : ICommand
    {
        public void Execute(IAppointmentService appointmentService, string[] commandArgs)
        {
            if (commandArgs.Length < 2)
            {
                Console.WriteLine("Usage: KEEP HH:mm");
                return;
            }

            if (TimeSpan.TryParseExact(commandArgs[1], "HH:mm", CultureInfo.InvariantCulture, out TimeSpan parsedTime))
            {
                appointmentService.KeepTimeSlotForAnyDayAsync(parsedTime);
            }
            else
            {
                Console.WriteLine("Invalid time format.");
            }
        }
    }

}
