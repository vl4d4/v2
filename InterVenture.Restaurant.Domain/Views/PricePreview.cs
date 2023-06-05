using Microsoft.EntityFrameworkCore;

namespace InterVenture.Restaurant.Domain.Views;

public class PricePreview
{
    public int Id { get; set; }
    public Guid ShoppingCartId { get; set; }
    public double Total { get; set; }
}