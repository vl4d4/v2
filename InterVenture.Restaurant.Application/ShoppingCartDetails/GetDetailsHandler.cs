namespace InterVenture.Restaurant.Application.ShoppingCartDetails;

public record GetDetails(int Id) : IRequest<GetDetailsResponse>;

public class GetDetailsResponse
{
    public double OriginalPrice { get; set; }
    public double? Discount { get; set; }
    public double Total { get; set; }
}

public class GetDetailsHandler : IRequestHandler<GetDetails, GetDetailsResponse>
{
    private readonly RestaurantContext context;

    public GetDetailsHandler(RestaurantContext context)
    {
        this.context = context;
    }

    public async Task<GetDetailsResponse> Handle(GetDetails request, CancellationToken cancellationToken)
    {
        var details = await context.ShoppingCartDetails.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("");

        return new GetDetailsResponse
        {
            OriginalPrice = Math.Round(details.OriginalPrice, 2),
            Discount = Math.Round(details.Discount, 2),
            Total = Math.Round(details.Total, 2)
        };
    }
}