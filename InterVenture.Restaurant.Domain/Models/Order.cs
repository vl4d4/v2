namespace InterVenture.Restaurant.Domain.Models;

public class Order
{
    public Guid Id { get; set; }
    public int RestaurantId { get; set; }
    public List<ProductItem> ProductItems { get; set; } = default!;
    public double Total { get; set; }
}