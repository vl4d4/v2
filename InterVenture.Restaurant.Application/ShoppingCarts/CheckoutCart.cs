namespace InterVenture.Restaurant.Application.ShoppingCarts;

public record Checkout(int RestaurantId, int Id, Guid OrderId) : IRequest;

internal sealed class CheckoutCartHandler : IRequestHandler<Checkout>
{
    private readonly RestaurantContext context;
    private readonly IPublisher publisher;

    public CheckoutCartHandler(RestaurantContext context, IPublisher publisher)
    {
        this.context = context;
        this.publisher = publisher;
    }

    public async Task Handle(Checkout request, CancellationToken cancellationToken)
    {
        var details = await context.ShoppingCartDetails
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("");

        CheckedOut @event = new(request.OrderId, request.RestaurantId, details.ShoppingCartId, details.Items, details.Total);
        await publisher.Publish(@event, cancellationToken);
    }
}

public record CheckedOut(Guid OrderId, int RestaurantId, Guid ShoppingCart, IEnumerable<ProductItem> Items, double Total) : INotification;