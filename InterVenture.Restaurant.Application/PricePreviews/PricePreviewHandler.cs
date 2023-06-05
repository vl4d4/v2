namespace InterVenture.Restaurant.Application.PricePreview;

internal sealed class PricePreviewHandler :
    INotificationHandler<ShoppingCartOpened>,
    INotificationHandler<ProductItemAdded>,
    INotificationHandler<ProductItemRemoved>,
    INotificationHandler<ShoppingCartConfirmed>,
    INotificationHandler<ShoppingCartCanceled>
{
    private readonly RestaurantContext context;

    public PricePreviewHandler(RestaurantContext context)
    {
        this.context = context;
    }

    public async Task Handle(ShoppingCartOpened notification, CancellationToken cancellationToken)
    {
        var existing = await context.PricePreviews.FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken);
        if (existing is not null)
        {
            throw new Exception($"Shopping cart with ID: {notification.ShoppingCartId} already opened");
        }

        Domain.Views.PricePreview readModel = new()
        {
            ShoppingCartId = notification.ShoppingCartId,
            Total = 0,
        };
        await context.PricePreviews.AddAsync(readModel, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(ProductItemAdded notification, CancellationToken cancellationToken)
    {
        var preview = await context.PricePreviews.FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");
        preview.Total += notification.ProductItem.Price;
        context.PricePreviews.Update(preview);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(ProductItemRemoved notification, CancellationToken cancellationToken)
    {
        var preview = await context.PricePreviews.FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");
        preview.Total -= notification.ProductItem.Price;
        context.PricePreviews.Update(preview);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(ShoppingCartConfirmed notification, CancellationToken cancellationToken)
    {
        var preview = await context.PricePreviews.FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");
        context.PricePreviews.Remove(preview);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(ShoppingCartCanceled notification, CancellationToken cancellationToken)
    {
        var preview = await context.PricePreviews.FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");
        context.PricePreviews.Remove(preview);
        await context.SaveChangesAsync(cancellationToken);
    }
}