namespace InterVenture.Restaurant.Application.Orders;

public record GetOrders(int RestaurantId) : IRequest<GetOrdersResponse>;

public record GetOrdersResponse(List<Order> Orders);

internal sealed class GetOrdersHandler : IRequestHandler<GetOrders, GetOrdersResponse>
{
    private readonly RestaurantContext context;

    public GetOrdersHandler(RestaurantContext context)
    {
        this.context = context;
    }

    public async Task<GetOrdersResponse> Handle(GetOrders request, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(x => x.ProductItems)
            .Where(x => x.RestaurantId == request.RestaurantId)
            .ToListAsync(cancellationToken)
            ?? throw new Exception(@"Orders not found");

        return new GetOrdersResponse(orders);
    }
}