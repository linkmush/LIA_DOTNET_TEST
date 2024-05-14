using LIA_DOTNET_TEST.Database;
using LIA_DOTNET_TEST.Models;
using LIA_DOTNET_TEST.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_LIA.TEST.Repository;

public class BookingRepository_test
{
    [Fact]
    public void Seed_WhenTimeSlotsTableIsEmpty_ShouldSeedTimeSlots()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase($"{nameof(BookingRepository_test)}_{Guid.NewGuid()}")
            .Options;

        using (var context = new Context(options))
        {
            var repository = new BookingRepository(context);

            // Act
            repository.Seed();

            // Assert
            Assert.NotEmpty(context.TimeSlots);
        }
    }

    [Fact]
    public void ShouldGetAllTimeSlots()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase($"{nameof(ShouldGetAllTimeSlots)}_{Guid.NewGuid()}")
            .Options;

        using (var context = new Context(options))
        {
            // Seed databasen med samples timeslot
            SeedDatabase(context);

            var repository = new BookingRepository(context);

            // Act
            var timeSlots = repository.GetAllTimeSlots();

            // Assert
            Assert.NotNull(timeSlots);
            Assert.NotEmpty(timeSlots);
            Assert.Equal(4, timeSlots.Count);
        }
    }

    [Fact]
    public void ShouldGetAllBookings_IncludedWithTimeSlots_AndUser()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase($"{nameof(ShouldGetAllBookings_IncludedWithTimeSlots_AndUser)}_{Guid.NewGuid()}")
            .Options;

        using (var context = new Context(options))
        {
            // Seed databasen med samples timeslot
            SeedDatabase(context);

            var repository = new BookingRepository(context);

            // Act
            var bookings = repository.GetAllBookings();

            // Assert
            Assert.NotNull(bookings);
            Assert.NotEmpty(bookings);
        }
    }

    [Fact]
    public void ShouldSaveBookingToDataBase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase($"{nameof(ShouldSaveBookingToDataBase)}_{Guid.NewGuid()}")
            .Options;

        using (var context = new Context(options))
        {
            var repository = new BookingRepository(context);
            var bookingToAdd = new Booking 
            { 
                Id = 1, Day = 1, 
                TimeSlot = new TimeSlot { Id = 1, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(12, 0, 0) }, 
                User = new User { Id = 1, Name = "User 1" } 
            };

            // Act
            repository.SaveBooking(bookingToAdd);

            // Assert
            var savedBooking = context.Bookings.FirstOrDefault(x => x.Id == 1);
            Assert.NotNull(savedBooking);
            Assert.Equal(1, savedBooking.Id);
        }
    }

    [Fact]
    public void ShouldRemoveBookingFromDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase($"{nameof(ShouldRemoveBookingFromDatabase)}_{Guid.NewGuid()}")
            .Options;

        using (var context = new Context(options))
        {
            var repository = new BookingRepository(context);
            var bookingToAdd = new Booking 
            { 
                Id = 1, Day = 1, 
                TimeSlot = new TimeSlot { Id = 1, StartTime = new TimeSpan(9, 0, 0), 
                EndTime = new TimeSpan(12, 0, 0) }, 
                User = new User { Id = 1, Name = "User 1" } 
            };

            // Add the booking to the database
            repository.SaveBooking(bookingToAdd);

            // Act
            var isBookingRemoved = repository.RemoveBooking(bookingToAdd);

            // Assert
            Assert.True(isBookingRemoved);
            var removedBooking = context.Bookings.FirstOrDefault(x => x.Id == 1);
            Assert.Null(removedBooking);
        }
    }

    [Fact]
    public void ShouldUpdateBookingInDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase($"{nameof(ShouldUpdateBookingInDatabase)}_{Guid.NewGuid()}")
            .Options;

        using (var context = new Context(options))
        {
            var repository = new BookingRepository(context);

            // Seed databasen med sample data
            SeedDatabase(context);

            var updatedBooking = new Booking
            {
                Id = 1,
                Day = 2,
                TimeSlot = new TimeSlot { Id = 2 }
            };

            // Act
            repository.UpdateBooking(1, updatedBooking);

            // Assert
            var updatedBookingFromDb = context.Bookings.FirstOrDefault(x => x.Id == 1);
            Assert.NotNull(updatedBookingFromDb);
            Assert.Equal(2, updatedBookingFromDb.Day);
            Assert.Equal(2, updatedBookingFromDb.TimeSlot!.Id);
        }
    }

    private static void SeedDatabase(Context context)
    {
        // Skapa samples data för bookings, timeslots och user.
        var timeSlots = new[]
        {
            new TimeSlot { Id = 1, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
            new TimeSlot { Id = 2, StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(14, 0, 0) },
            new TimeSlot { Id = 3, StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(16, 0, 0) },
            new TimeSlot { Id = 4, StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(20, 0, 0) },
        };

        var users = new[]
        {
            new User { Id = 1, Name = "User 1" },
        };

        var bookings = new[]
        {
            new Booking { Id = 1, Day = 1, TimeSlot = timeSlots[0], User = users[0] },
        };

        // Add entiteter till context och save changes
        context.TimeSlots.AddRange(timeSlots);
        context.User.AddRange(users);
        context.Bookings.AddRange(bookings);
        context.SaveChanges();
    }
}
