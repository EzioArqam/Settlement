using System.ComponentModel.DataAnnotations;

namespace SettlementService.DTOs;

public class CreateBookingDTO
{
    [Required]
    public string Name { get; set; }
    public string BookingTime { get; set; }

}
