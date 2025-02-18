namespace cakeDelivery.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentStatus { get; set; }

    public Order Order { get; set; }
}