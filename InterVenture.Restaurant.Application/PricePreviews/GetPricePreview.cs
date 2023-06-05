namespace InterVenture.Restaurant.Application.PricePreview;

public record GetPricePreview(int Id) : IRequest<PricePreviewResponse>;

public class PricePreviewResponse
{
    public double Total { get; set; }
}

internal sealed class GetPricePreviewHandler : IRequestHandler<GetPricePreview, PricePreviewResponse>
{
    private readonly RestaurantContext context;

    public GetPricePreviewHandler(RestaurantContext context)
    {
        this.context = context;
    }

    public async Task<PricePreviewResponse> Handle(GetPricePreview request, CancellationToken cancellationToken)
    {
        var preview = await context.PricePreviews.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new Exception("");

        return new PricePreviewResponse
        {
            Total = Math.Round(preview.Total, 2)
        };
    }
}