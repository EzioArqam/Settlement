using Microsoft.EntityFrameworkCore;
using SettlementService.Data;
using SettlementService.Entities;

namespace SettlementService.Repositories;

public class BookingRepository : IBookingRepository
{
    // Injecting DB Context here for separation of Data Layer and Logic Layer
    private readonly BookingsDbContext _context;

    public BookingRepository(BookingsDbContext context)
    {
        _context = context;
    }

    // Method to get overlapping bookings
    public async Task<List<Booking>> GetBookingsAtTime(TimeSpan bookingTime)
    {
        return await _context.Bookings
            .Where(b => (b.BookingTime >= bookingTime && b.BookingTime < bookingTime.Add(TimeSpan.FromHours(1))) ||
                        (bookingTime >= b.BookingTime && bookingTime < b.BookingTime.Add(TimeSpan.FromHours(1))))
            .ToListAsync();
    }

    public void AddBooking(Booking booking)
    {
        _context.Bookings.Add(booking);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
