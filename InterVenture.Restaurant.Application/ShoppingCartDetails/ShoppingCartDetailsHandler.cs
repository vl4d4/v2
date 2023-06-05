namespace InterVenture.Restaurant.Application.ShoppingCartDetails;

internal sealed class ShoppingCartDetailsHandler :
    INotificationHandler<ShoppingCartOpened>,
    INotificationHandler<ProductItemAdded>,
    INotificationHandler<ProductItemRemoved>,
    INotificationHandler<ShoppingCartConfirmed>
{
    private readonly RestaurantContext context;
    private readonly IDiscountCalculator calculator;

    public ShoppingCartDetailsHandler(RestaurantContext context, IDiscountCalculator calculator)
    {
        this.context = context;
        this.calculator = calculator;
    }
    public async Task Handle(ShoppingCartOpened notification, CancellationToken cancellationToken)
    {
        var existing = await context.ShoppingCartDetails.FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken);
        if (existing is not null)
        {
            throw new Exception($"Shopping cart with ID: {notification.ShoppingCartId} already opened");
        }

        ShoppingCartDetail readModel = new()
        {
            ShoppingCartId = notification.ShoppingCartId,
            Total = 0,
            Discount = 0,
            OriginalPrice = 0
        };
        await context.ShoppingCartDetails.AddAsync(readModel, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(ProductItemAdded notification, CancellationToken cancellationToken)
    {
        var details = await context.ShoppingCartDetails
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");

        details.Items.Add(notification.ProductItem);
        details.Total += notification.ProductItem.Price;
        details.OriginalPrice += notification.ProductItem.Price;
        context.ShoppingCartDetails.Update(details);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(ProductItemRemoved notification, CancellationToken cancellationToken)
    {
        var details = await context.ShoppingCartDetails
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");

        details.Items.Remove(notification.ProductItem);
        details.Total -= notification.ProductItem.Price;
        details.OriginalPrice -= notification.ProductItem.Price;
        context.ShoppingCartDetails.Update(details);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Handle(ShoppingCartConfirmed notification, CancellationToken cancellationToken)
    {
        var details = await context.ShoppingCartDetails.FirstOrDefaultAsync(x => x.ShoppingCartId == notification.ShoppingCartId, cancellationToken)
            ?? throw new Exception("");
        var restaurant = await context.Restaurants.FirstOrDefaultAsync(x => x.Id == notification.RestaurantId, cancellationToken)
            ?? throw new Exception("");
        details.Discount = calculator.Calculate(details.OriginalPrice, notification.ConfirmedAt, restaurant.Offset);
        details.OriginalPrice = details.OriginalPrice;
        details.Total = details.Discount > 0 ? details.OriginalPrice - details.Discount : details.OriginalPrice;
        context.ShoppingCartDetails.Update(details);
        await context.SaveChangesAsync(cancellationToken);
    }
}