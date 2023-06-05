namespace InterVenture.Restaurant.Domain.Models;

public class Meal
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public List<DishMeal> DishMeals { get; set; } = default!;
    public List<Dish> Dishes { get; set; } = default!;
}