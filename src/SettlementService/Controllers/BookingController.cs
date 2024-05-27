
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SettlementService.Data;
using SettlementService.DTOs;
using SettlementService.Entities;
using SettlementService.Repositories;

namespace SettlementService;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IBookingRepository _repository;

    public BookingController( IMapper mapper, IBookingRepository repository)
    {
        // Injecting Mapper and Booking Repository to be used in Controller.
        
        _mapper = mapper;
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<CreateBookingDTO>> CreateBooking(CreateBookingDTO bookingDTO)
    {

        // Checking for invalid time format

        if (!TimeSpan.TryParse(bookingDTO.BookingTime, out var bookingTime))
        {
            return BadRequest("Invalid booking time format.");
        }

        // Mapping DTO to Entity

        var booking = _mapper.Map<Booking>(bookingDTO);
        if (bookingTime < new TimeSpan(9, 0, 0) || bookingTime > new TimeSpan(16, 0, 0))
        {
            return BadRequest("Booking time is out of business hours (9am-5pm).");
        }

        // Consuming the injected repository for overlapping Bookings

        var overlappingBookings = await _repository.GetBookingsAtTime(bookingTime);

        if (overlappingBookings.Count >= 4)
        {
            return Conflict("All settlements at this booking time are reserved.");
        }
        // Consuming the repository for succesful insertion in DB

        _repository.AddBooking(booking);
        var result = await _repository.SaveChangesAsync();

        if (!result) return BadRequest("Could Not Update the DB");

        // Success case: Return the generated booking Id.

        return Ok(new { BookingId = booking.Id });
    }
}
