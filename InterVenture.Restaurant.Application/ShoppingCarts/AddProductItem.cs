namespace InterVenture.Restaurant.Application.ShoppingCarts;

public record AddProductItem(Guid ShoppingCartId, string Name, double Price) : IRequest;

internal sealed class AddProductItemHandler : IRequestHandler<AddProductItem>
{
    private readonly RestaurantContext context;
    private readonly IPublisher publisher;

    public AddProductItemHandler(RestaurantContext context, IPublisher publisher)
    {
        this.context = context;
        this.publisher = publisher;
    }

    public async Task Handle(AddProductItem request, CancellationToken cancellationToken)
    {
        var shoppingCart = await context.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == request.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");

        var productItem = new ProductItem
        {
            ShoppingCartId = request.ShoppingCartId,
            Name = request.Name,
            Price = request.Price
        };
        var entry = await context.ProductItems.AddAsync(productItem, cancellationToken);

        shoppingCart.Status = ShoppingCartStatus.Pending;
        shoppingCart.Add(entry.Entity);

        context.ShoppingCarts.Update(shoppingCart);
        await context.SaveChangesAsync(cancellationToken);

        ProductItemAdded @event = new(request.ShoppingCartId, entry.Entity);
        await publisher.Publish(@event, cancellationToken);
    }
}

public record ProductItemAdded(Guid ShoppingCartId, ProductItem ProductItem) : INotification;