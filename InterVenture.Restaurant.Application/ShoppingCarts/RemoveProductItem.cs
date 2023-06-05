namespace InterVenture.Restaurant.Application.ShoppingCarts;

public record RemoveProductItem(Guid ShoppingCartId, int ProductItemId) : IRequest;

internal sealed class RemoveProductItemHandler : IRequestHandler<RemoveProductItem>
{
    private readonly RestaurantContext context;
    private readonly IPublisher publisher;

    public RemoveProductItemHandler(RestaurantContext context, IPublisher publisher)
    {
        this.context = context;
        this.publisher = publisher;
    }

    public async Task Handle(RemoveProductItem request, CancellationToken cancellationToken)
    {
        var shoppingCart = await context.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == request.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");

        var productItem = await context.ProductItems
            .FirstOrDefaultAsync(x => x.Id == request.ProductItemId, cancellationToken)
            ?? throw new Exception("");

        context.ProductItems.Remove(productItem);

        shoppingCart.Status = ShoppingCartStatus.Pending;
        shoppingCart.Remove(productItem);

        context.Update(shoppingCart);
        await context.SaveChangesAsync(cancellationToken);

        ProductItemRemoved @event = new(request.ShoppingCartId, productItem);
        await publisher.Publish(@event, cancellationToken);
    }
}

public record ProductItemRemoved(Guid ShoppingCartId, ProductItem ProductItem) : INotification;