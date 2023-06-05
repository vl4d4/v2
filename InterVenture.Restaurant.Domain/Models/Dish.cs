namespace InterVenture.Restaurant.Domain.Models;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public List<DishMeal> DishMeals { get; set; } = default!;
    public List<Meal>? Meals { get; set; }
}