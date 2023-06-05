namespace InterVenture.Restaurant.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RestaurantContext>(options => options.UseSqlServer(configuration.GetConnectionString("RestaurantDB")));
}