namespace InterVenture.Restaurant.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<IAssemblyMarker>())
            .AddScoped<IIdGenerator, IdGenerator>()
            .AddScoped<IDiscountCalculator, DiscountCalculator>()
            .AddScoped<IMealCalculator, MealCalculator>();
}