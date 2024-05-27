using Microsoft.EntityFrameworkCore;
using SettlementService.Entities;

namespace SettlementService.Data;

public class BookingsDbContext : DbContext
{
    // Setting a DB Context (in-memory) for Bookings
    public BookingsDbContext(DbContextOptions options) : base(options){

    }
    public DbSet<Booking> Bookings { get; set; }
}
