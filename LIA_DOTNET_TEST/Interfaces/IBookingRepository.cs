using LIA_DOTNET_TEST.Models;
using Microsoft.Identity.Client;

namespace LIA_DOTNET_TEST.Interfaces
{
    public interface IBookingRepository
    {
        public ICollection<Booking> GetAllBookings();
        public ICollection<TimeSlot> GetAllTimeSlots();
        public void SaveBooking(Booking booking);
        public bool RemoveBooking(Booking booking);
        public void UpdateBooking(int id, Booking booking);
        public void Seed();
    }
}
