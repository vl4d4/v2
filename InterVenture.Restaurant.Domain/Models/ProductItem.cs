namespace InterVenture.Restaurant.Domain.Models;

public class ProductItem
{
    public int Id { get; set; }
    public Guid ShoppingCartId { get; set; }
    public double Price { get; set; }
    public string Name { get; set; } = default!;
}