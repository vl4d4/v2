namespace InterVenture.Restaurant.Domain.Models;

public class DishMeal
{
    public int Id { get; set; }
    public int DishId { get; set; }
    public int? MealId { get; set; }
    public Dish Dish { get; set; } = default!;
    public Meal? Meal { get; set; }
}