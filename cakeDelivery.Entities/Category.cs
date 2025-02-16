namespace cakeDelivery.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryImageUrl { get; set; }

    public ICollection<Cake> Cakes { get; set; }
}