using LIA_DOTNET_TEST.Database;
using LIA_DOTNET_TEST.Interfaces;
using LIA_DOTNET_TEST.Models;
using Microsoft.AspNetCore.Mvc;

namespace LIA_DOTNET_TEST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {

        readonly IBookingRepository _bookingRepository;
        private readonly Context _context;

        public BookingController(IBookingRepository bookingRepository, Context context)
        {
            _bookingRepository = bookingRepository;
            _context = context;
        }

        [HttpPost]
        public ActionResult<Booking> AddBoking(Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var timeSlot = _context.TimeSlots.FirstOrDefault(ts => ts.Id == booking.TimeSlot!.Id);

                    if (timeSlot != null)
                    {
                        booking.TimeSlot = timeSlot;

                        _bookingRepository.SaveBooking(booking);

                        return Ok(booking);
                    }
                    else
                    {
                        return BadRequest("Invalid TimeSlot ID");
                    }
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return BadRequest(new { exception.Message });
            }
        }


        [HttpGet]
        public ActionResult<ICollection<Booking>> GetAll()
        {
            try
            {
                ICollection<Booking> bookings = _bookingRepository.GetAllBookings();

                return Ok(bookings);
            }
            catch (Exception exception)
            {

                return BadRequest(new { exception.Message });
            }

        }


        [HttpGet("timeslots")]
        public ActionResult<ICollection<TimeSlot>> GetTimeSlots()
        {
            try
            {
                ICollection<TimeSlot> timeSlots = _bookingRepository.GetAllTimeSlots();

                return Ok(timeSlots);
            }
            catch (Exception exception)
            {

                return BadRequest(new { exception.Message });
            }

        }

        [HttpDelete]
        public ActionResult Delete(Booking booking)
        {
            try
            {
                var isDeleted = _bookingRepository.RemoveBooking(booking);
                if (isDeleted)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception exception)
            {

                return BadRequest(new { exception.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, Booking updatedBooking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    updatedBooking.Id = id;

                    _bookingRepository.UpdateBooking(id, updatedBooking);

                    return Ok("Booking updated successfully");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}