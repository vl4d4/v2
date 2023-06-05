namespace InterVenture.Restaurant.Application.Restaurants;

public record GetRestaurants : IRequest<GetRestaurantsResponse>;

public class ViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public record GetRestaurantsResponse(IEnumerable<ViewModel> Restaurants);

internal sealed class GetRestaurantsHandler : IRequestHandler<GetRestaurants, GetRestaurantsResponse>
{
    private readonly RestaurantContext context;

    public GetRestaurantsHandler(RestaurantContext context)
    {
        this.context = context;
    }

    public async Task<GetRestaurantsResponse> Handle(GetRestaurants request, CancellationToken cancellationToken)
    {
        var restaurants = await context.Restaurants.ToListAsync(cancellationToken);

        var viewModel = restaurants.Select(x => new ViewModel
        {
            Id = x.Id,
            Name = x.Name,
        });

        return new GetRestaurantsResponse(viewModel);
    }
}