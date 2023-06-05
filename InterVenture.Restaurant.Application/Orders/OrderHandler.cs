namespace InterVenture.Restaurant.Application.Orders;

internal sealed class OrderHandler : INotificationHandler<CheckedOut>
{
    private readonly RestaurantContext context;

    public OrderHandler(RestaurantContext context)
    {
        this.context = context;
    }

    public async Task Handle(CheckedOut notification, CancellationToken cancellationToken)
    {
        Order entity = new()
        {
            Id = notification.OrderId,
            RestaurantId = notification.RestaurantId,
            ProductItems = notification.Items.ToList(),
            Total = notification.Total,
        };
        await context.Orders.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}