using LIA_DOTNET_TEST.Database;
using LIA_DOTNET_TEST.Interfaces;
using LIA_DOTNET_TEST.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace LIA_DOTNET_TEST.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly Context _context;

        //Jag ändrade min kod för att använda Dependency Injection (DI), vilket gjorde den mer löst kopplad och lättare att testa
        //samtidigt som jag bibehöll principerna för Single Responsibility Principle (SRP).

        public BookingRepository(Context context)
        {
            _context = context;
        }

        public void Seed()
        {
            //kontrollera om timeslots är tom, om den är tom skapa ny annars skapa inte ny timeslots i databas
            //Om jag inte kontroller detta duplikeras tiderna varje gång jag startar projektet.
            if (!_context.TimeSlots.Any())
            {
                ICollection<TimeSlot> timeSlots = ProduceTimeSlots();
                _context.TimeSlots.AddRange(timeSlots);
                _context.SaveChanges();
            }
        }

        public ICollection<Booking> GetAllBookings()
        {
            return _context.Bookings.Include(x => x.User).Include(x => x.TimeSlot).ToList();
        }

        public ICollection<TimeSlot> GetAllTimeSlots()
        {
            return _context.TimeSlots.ToList();
        }


        private static ICollection<TimeSlot> ProduceTimeSlots()
        {
            return new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                         StartTime = new TimeSpan(9, 0,0),
                         EndTime = new TimeSpan(12, 0,0),
                    },
                    new TimeSlot()
                    {
                         StartTime = new TimeSpan(12, 0,0),
                         EndTime = new TimeSpan(14, 0,0),
                    },
                    new TimeSlot()
                    {
                         StartTime = new TimeSpan(14, 0,0),
                         EndTime = new TimeSpan(16, 0,0),
                    },
                    new TimeSlot()
                    {
                         StartTime = new TimeSpan(16, 0,0),
                         EndTime = new TimeSpan(20, 0,0),
                    },

                };
        }

        public void SaveBooking(Booking booking)
        {
            try
            {
                // Kontrollerar om det finns en bokning med samma booking ID
                var existingBooking = _context.Bookings.FirstOrDefault(x => x.Id == booking.Id);

                // Om existing timeslot inte är hittad lägg till boknigen i databasen.
                if (existingBooking == null)
                {
                    _context.Bookings.Add(booking);
                    _context.SaveChanges();

                    Debug.WriteLine($"Booking saved successfully: {booking.Id}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred in the SaveBooking method: {ex.Message}");
            }
        }

        public bool RemoveBooking(Booking booking)
        {
            try
            {

                // Hitta bokningen i databasen med det specifika ID:et
                var removeBooking = _context.Bookings.Include(x => x.User).FirstOrDefault(x => x.Id == booking.Id);

                // Om bokningen med det specifika ID:et existerar
                if (removeBooking != null)
                {
                    // Tar bort bokningen från databasen
                    _context.Bookings.Remove(removeBooking);

                    // Tar även bort den användare som är kopplad till denna bokning från databasen
                    if (removeBooking.User != null)
                    {
                        _context.User.Remove(removeBooking.User);
                    }

                    //  Spara ändringar i databasen
                    _context.SaveChanges();

                    // return true om det gick att ta bort
                    return true;
                }
                else
                {
                    // Om bokningen med det specifika ID:et inte existerar retunera false
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred in the RemoveBooking method: {ex.Message}");

                // retunerar false om det inte fungerade att ta bort bokningen.
                return false;
            }
        }

        public void UpdateBooking(int id, Booking booking)
        {
            try
            {
                var existingBooking = _context.Bookings.Include(x => x.TimeSlot).FirstOrDefault(x => x.Id == id);

                if (existingBooking == null)
                {
                    Debug.WriteLine($"Booking with id {id} not found.");
                    return;
                }

                // Kontrollera om en annan bokning redan finns för samma dag och timeslot
                var conflictingBooking = _context.Bookings.FirstOrDefault(x => x.Day == booking.Day && x.TimeSlot!.Id == booking.TimeSlot!.Id && x.Id != id);

                if (conflictingBooking != null)
                {
                    Debug.WriteLine($"A booking already exists for day {booking.Day} and timeslot {booking.TimeSlot!.Id}.");
                    return;
                }

                existingBooking.Day = booking.Day;

                // Hämta befintlig TimeSlot från databasen
                var existingTimeSlot = _context.TimeSlots.FirstOrDefault(x => x.Id == booking.TimeSlot!.Id);

                if (existingTimeSlot != null)
                {
                    existingBooking.TimeSlot = existingTimeSlot;
                }
                else
                {
                    Debug.WriteLine($"TimeSlot with id {booking.TimeSlot!.Id} not found.");
                    return;
                }

                _context.SaveChanges();
                Debug.WriteLine($"Booking updated successfully: {booking.Id}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred in the UpdateBooking method: {ex.Message}");
            }
        }
    }
}
