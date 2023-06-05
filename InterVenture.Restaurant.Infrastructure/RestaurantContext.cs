namespace InterVenture.Restaurant.Infrastructure;

public class RestaurantContext : DbContext
{
    public RestaurantContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Domain.Models.Restaurant> Restaurants => Set<Domain.Models.Restaurant>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Meal> Meals => Set<Meal>();
    public DbSet<DishMeal> DishMeals => Set<DishMeal>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<ProductItem> ProductItems => Set<ProductItem>();
    public DbSet<PricePreview> PricePreviews => Set<PricePreview>();
    public DbSet<ShoppingCartDetail> ShoppingCartDetails => Set<ShoppingCartDetail>();
}