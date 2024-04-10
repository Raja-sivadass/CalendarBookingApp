using CalendarBookingApp.Commands;
using Moq;
using Xunit;

namespace CalendarBookingApp.Tests
{
    public class CommandTests
    {
        [Fact]
        public async Task AddCommand_Execute_ValidInput_CallsAddAppointmentAsync()
        {
            // Arrange
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var command = new AddCommand();

            // Act
            command.Execute(appointmentServiceMock.Object, new string[] { "ADD", "25/03", "12:00" });

            // Assert
            appointmentServiceMock.Verify(s => s.AddAppointmentAsync(It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCommand_Execute_ValidInput_CallsDeleteAppointmentAsync()
        {
            // Arrange
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var command = new DeleteCommand();

            // Act
            command.Execute(appointmentServiceMock.Object, new string[] { "DELETE", "25/03", "12:00" });

            // Assert
            appointmentServiceMock.Verify(s => s.DeleteAppointmentAsync(It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task FindCommand_Execute_ValidInput_CallsFindFreeTimeSlotsAsync()
        {
            // Arrange
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var command = new FindCommand();

            // Act
            command.Execute(appointmentServiceMock.Object, new string[] { "FIND", "25/03" });

            // Assert
            appointmentServiceMock.Verify(s => s.FindFreeTimeSlotsAsync(It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task KeepCommand_Execute_ValidInput_CallsKeepTimeSlotForAnyDayAsync()
        {
            // Arrange
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var command = new KeepCommand();

            // Act
            command.Execute(appointmentServiceMock.Object, new string[] { "KEEP", "12:00" });

            // Assert
            appointmentServiceMock.Verify(s => s.KeepTimeSlotForAnyDayAsync(It.IsAny<TimeSpan>()), Times.Once);
        }

        // Add more test methods for edge cases, invalid input, etc.
    }
}
