namespace InterVenture.Restaurant.API.Requests;

public class AddItemRequest
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
}