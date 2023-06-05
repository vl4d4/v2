namespace InterVenture.Restaurant.Application.Orders;

public record GetOrder(Guid OrderId) : IRequest<GetOrderResponse>;

public record GetOrderResponse(Order Order);

internal sealed class GetOrderHandler : IRequestHandler<GetOrder, GetOrderResponse>
{
    private readonly RestaurantContext context;

    public GetOrderHandler(RestaurantContext context)
    {
        this.context = context;
    }

    public async Task<GetOrderResponse> Handle(GetOrder request, CancellationToken cancellationToken)
    {
        var order = await context.Orders
            .Include(x => x.ProductItems)
            .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken)
            ?? throw new Exception(@"Order not found");

        return new GetOrderResponse(order);
    }
}