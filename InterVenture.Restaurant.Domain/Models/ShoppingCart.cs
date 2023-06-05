namespace InterVenture.Restaurant.Domain.Models;

public class ShoppingCart
{
    public static ShoppingCart Empty(Guid id, int restaurantId) => new(id, restaurantId);

    private ShoppingCart(Guid id, int restaurantId)
    {
        Id = id;
        RestaurantId = restaurantId;
        Status = ShoppingCartStatus.Empty;
        ProductItems = new();
    }

    public Guid Id { get; set; }
    public int RestaurantId { get; set; }
    public List<ProductItem> ProductItems { get; set; }
    public ShoppingCartStatus Status { get; set; }

    public void Add(ProductItem item) => ProductItems.Add(item);
    public void Remove(ProductItem item) => ProductItems.Remove(item);
}