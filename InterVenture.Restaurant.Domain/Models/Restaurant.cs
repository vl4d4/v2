namespace InterVenture.Restaurant.Domain.Models;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int Offset { get; set; }
}