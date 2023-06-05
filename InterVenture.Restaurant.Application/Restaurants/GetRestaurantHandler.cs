namespace InterVenture.Restaurant.Application.Restaurants;

public record GetRestaurant(int Id) : IRequest<ViewModel>;

internal sealed class GetRestaurantHandler : IRequestHandler<GetRestaurant, ViewModel>
{
    private readonly RestaurantContext context;

    public GetRestaurantHandler(RestaurantContext context)
    {
        this.context = context;
    }

    public async Task<ViewModel> Handle(GetRestaurant request, CancellationToken cancellationToken)
    {
        var entity = await context.Restaurants.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception($"Invalid restaurant id: {request.Id}.");

        return new ViewModel()
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}