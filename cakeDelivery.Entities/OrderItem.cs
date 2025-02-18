namespace cakeDelivery.Entities;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int CakeId { get; set; }
    public int SizeId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }

    public Order Order { get; set; }
    public Cake Cake { get; set; }
    public Size Size { get; set; }
}
