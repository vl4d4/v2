namespace InterVenture.Restaurant.Domain.Views;

public class ShoppingCartDetail
{
    public int Id { get; set; }
    public Guid ShoppingCartId { get; set; }
    public List<ProductItem> Items { get; set; } = default!;
    public double OriginalPrice { get; set; }
    public double Discount { get; set; }
    public double Total { get; set; }
}