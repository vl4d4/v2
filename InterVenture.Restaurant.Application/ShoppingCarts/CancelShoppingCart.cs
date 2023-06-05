namespace InterVenture.Restaurant.Application.ShoppingCarts;

public record CancelShoppingCart(Guid ShoppingCartId) : IRequest;

internal sealed class CancelShoppingCartHandler : IRequestHandler<CancelShoppingCart>
{
    private readonly RestaurantContext context;
    private readonly IPublisher publisher;

    public CancelShoppingCartHandler(RestaurantContext context, IPublisher publisher)
    {
        this.context = context;
        this.publisher = publisher;
    }

    public async Task Handle(CancelShoppingCart request, CancellationToken cancellationToken)
    {
        var shoppingCart = await context.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == request.ShoppingCartId, cancellationToken)
            ?? throw new Exception(@"Shopping cart with ID: {request.shoppingCartId} not found");
        context.ShoppingCarts.Remove(shoppingCart);
        await context.SaveChangesAsync(cancellationToken);

        ShoppingCartCanceled @event = new(request.ShoppingCartId, DateTimeOffset.Now);
        await publisher.Publish(@event, cancellationToken);
    }
}

public record ShoppingCartCanceled(Guid ShoppingCartId, DateTimeOffset CanceledAt) : INotification;