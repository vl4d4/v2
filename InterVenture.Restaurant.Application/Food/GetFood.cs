namespace InterVenture.Restaurant.Application.Food;

public record GetDishMeals : IRequest<IEnumerable<FoodResponse>>;

public class FoodResponse
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public int ItemId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ItemType Type { get; set; }

    public string Name { get; set; } = default!;

    public double Price { get; set; }
}

internal sealed class GetFoodHandler : IRequestHandler<GetDishMeals, IEnumerable<FoodResponse>>
{
    private readonly RestaurantContext context;
    private readonly IMealCalculator calculator;

    public GetFoodHandler(RestaurantContext context, IMealCalculator calculator)
    {
        this.context = context;
        this.calculator = calculator;
    }

    public async Task<IEnumerable<FoodResponse>> Handle(GetDishMeals request, CancellationToken cancellationToken)
    {
        var food = await context.DishMeals
            .Include(x => x.Dish)
            .Include(x => x.Meal)
            .ToListAsync(cancellationToken);

        var viewModel = new List<FoodResponse>();

        foreach (var item in food)
        {
            if (!viewModel.Any(x => x.ItemId == item.DishId && x.Type == ItemType.Dish))
            {
                var dish = new FoodResponse
                {
                    Id = item.Id,
                    ItemId = item.DishId,
                    Type = ItemType.Dish,
                    Price = item.Dish.Price,
                    Name = item.Dish.Name
                };
                viewModel.Add(dish);
            }

            if (item.MealId.HasValue)
            {
                if (!viewModel.Any(x => x.ItemId == item.MealId.Value && x.Type == ItemType.Meal))
                {
                    var meal = new FoodResponse
                    {
                        Id = item.Id,
                        ItemId = item.MealId.Value,
                        Type = ItemType.Meal,
                        Price = calculator.Calculate(context.DishMeals
                            .Where(x => x.MealId == item.MealId)
                            .Select(x => x.Dish)
                            .Sum(x => x.Price)),
                        Name = item.Meal!.Name
                    };
                    viewModel.Add(meal);
                }
            }
        }

        return viewModel.OrderBy(x => x.Type);
    }
}