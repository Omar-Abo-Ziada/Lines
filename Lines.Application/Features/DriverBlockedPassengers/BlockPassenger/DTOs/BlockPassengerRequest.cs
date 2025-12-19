using System.ComponentModel.DataAnnotations;

namespace Lines.Application.Features.DriverBlockedPassengers.BlockPassenger.DTOs;

public class BlockPassengerRequest
{
    [Required]
    public Guid PassengerId { get; set; }
    
    [MaxLength(500)]
    public string? Reason { get; set; }
}

