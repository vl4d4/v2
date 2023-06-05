namespace InterVenture.Restaurant.Application.ShoppingCarts;

public record OpenShoppingCart(Guid ShoppingCartId, int RestaurantId) : IRequest;

public class OpenShoppingCartHandler : IRequestHandler<OpenShoppingCart>
{
    private readonly RestaurantContext context;
    private readonly IPublisher publisher;

    public OpenShoppingCartHandler(RestaurantContext context, IPublisher publisher)
    {
        this.context = context;
        this.publisher = publisher;
    }

    public async Task Handle(OpenShoppingCart request, CancellationToken cancellationToken)
    {
        var entity = ShoppingCart.Empty(request.ShoppingCartId, request.RestaurantId);

        await context.ShoppingCarts.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        ShoppingCartOpened @event = new(request.ShoppingCartId);
        await publisher.Publish(@event, cancellationToken);
    }
}

public record ShoppingCartOpened(Guid ShoppingCartId) : INotification;