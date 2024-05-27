namespace SettlementService.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TimeSpan BookingTime { get; set; }

    public bool HasName() => Name != ""; 
}
