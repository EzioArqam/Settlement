using SettlementService.Entities;

namespace SettlementService.Repositories;

public interface IBookingRepository
{
    Task<List<Booking>> GetBookingsAtTime(TimeSpan bookingTime);
    void AddBooking(Booking booking);

    Task<bool> SaveChangesAsync();
}
