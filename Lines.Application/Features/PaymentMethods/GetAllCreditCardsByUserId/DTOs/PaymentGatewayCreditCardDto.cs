namespace Lines.Application.Features.PaymentMethods.GetAllCreditCardsByUserId.DTOs;

public class PaymentGatewayCreditCardDto
{
    public string Id { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public string Last4 { get; set; } = default!;
    public long ExpMonth { get; set; }
    public long ExpYear { get; set; }
    public bool IsDefault { get; set; }
}