namespace cakeDelivery.Entities;

public class Size
{
    public int SizeId { get; set; }
    public string SizeName { get; set; }
    public decimal PriceMultiplier { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}