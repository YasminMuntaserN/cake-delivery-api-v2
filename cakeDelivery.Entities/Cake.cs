namespace cakeDelivery.Entities;

public class Cake
{
    public int CakeId { get; set; }
    public string CakeName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public Category Category { get; set; }
 //   public ICollection<OrderItem> OrderItems { get; set; }
}