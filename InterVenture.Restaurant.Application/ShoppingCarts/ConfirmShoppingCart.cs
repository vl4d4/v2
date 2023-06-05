namespace InterVenture.Restaurant.Application.ShoppingCarts;

public record ConfirmShoppingCart(Guid ShoppingCartId) : IRequest;

internal sealed class ConfirmShoppingCartHandler : IRequestHandler<ConfirmShoppingCart>
{
    private readonly RestaurantContext context;
    private readonly IPublisher publisher;

    public ConfirmShoppingCartHandler(RestaurantContext context, IPublisher publisher)
    {
        this.context = context;
        this.publisher = publisher;
    }

    public async Task Handle(ConfirmShoppingCart request, CancellationToken cancellationToken)
    {
        var shoppingCart = await context.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == request.ShoppingCartId, cancellationToken)
            ?? throw new Exception(@"Shopping cart with ID: {request.shoppingCartId} not found");

        shoppingCart.Status = ShoppingCartStatus.Confirmed;
        context.ShoppingCarts.Update(shoppingCart);
        await context.SaveChangesAsync(cancellationToken);

        ShoppingCartConfirmed @event = new(request.ShoppingCartId, DateTimeOffset.Now, shoppingCart.RestaurantId);
        await publisher.Publish(@event, cancellationToken);
    }
}

public record ShoppingCartConfirmed(Guid ShoppingCartId, DateTimeOffset ConfirmedAt, int RestaurantId) : INotification;