var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/restaurants", async (ISender sender) =>
{
    GetRestaurants query = new();
    var viewModel = await sender.Send(query);

    return Results.Ok(viewModel);
});
app.MapGet("/restaurants/{restaurantId}", async (int restaurantId, ISender sender) =>
{
    GetRestaurant query = new(restaurantId);
    var viewModel = await sender.Send(query);

    return Results.Ok(viewModel);
});


app.MapGet("/food", async (ISender sender) =>
{
    GetDishMeals query = new();
    var viewModel = await sender.Send(query);

    return Results.Ok(viewModel);

});

app.MapPost("/shopping-carts", async (OpenCart request, IIdGenerator idGenerator, ISender sender) =>
{
    var shoppingCartId = idGenerator.New();
    OpenShoppingCart command = new(shoppingCartId, request.RestaurantId);
    await sender.Send(command);

    return Results.Created($"/restaurants/{request.RestaurantId}/shopping-carts/{shoppingCartId}", null);
});

app.MapPost("/shopping-carts/{shoppingCartId}/items",
    async (Guid shoppingCartId, AddItemRequest request, ISender sender) =>
    {
        AddProductItem command = new(shoppingCartId, request.Name, request.Price);
        await sender.Send(command);

        return Results.Ok();
    });

app.MapDelete("/shopping-carts/{shoppingCartId}/items/{itemId}",
    async (Guid shoppingCartId, int itemId, ISender sender) =>
    {
        RemoveProductItem command = new(shoppingCartId, itemId);
        await sender.Send(command);

        return Results.NoContent();
    });

app.MapPut("/shopping-carts/{shoppingCartId}/confirm",
    async (Guid shoppingCartId, ISender sender) =>
    {
        ConfirmShoppingCart command = new(shoppingCartId);
        await sender.Send(command);

        return Results.NoContent();
    });

app.MapDelete("/shopping-carts/{shoppingCartId}/cancel",
    async (Guid shoppingCartId, ISender sender) =>
    {
        CancelShoppingCart cancelShoppingCart = new(shoppingCartId);
        await sender.Send(cancelShoppingCart);

        return Results.NoContent();
    });

app.MapGet("price-preview/{id}", async (int id, ISender sender) =>
{
    GetPricePreview query = new(id);
    var viewModel = await sender.Send(query);

    return Results.Ok(viewModel);
});

app.MapGet("/shopping-cart-details/{id}", async (int id, ISender sender) =>
{
    GetDetails query = new(id);
    var viewModel = await sender.Send(query);

    return Results.Ok(viewModel);
});

app.MapPost("/shopping-cart-details/{id}/checkout",
    async (int id, InterVenture.Restaurant.API.Requests.Checkout request, IIdGenerator idGenerator, ISender sender) =>
    {
        var orderId = idGenerator.New();
        InterVenture.Restaurant.Application.ShoppingCarts.Checkout command = new(request.RestaurantId, id, orderId);
        await sender.Send(command);

        return Results.Accepted();
    });

app.MapGet("/restaurants/{restaurantId}/orders", async (int restaurantId, ISender sender) =>
{
    GetOrders query = new(restaurantId);
    var viewModel = await sender.Send(query);

    return Results.Ok(viewModel);
});

app.MapGet("/orders/{orderId}", async (int restaurantId, Guid orderId, ISender sender) =>
{
    GetOrder query = new(orderId);
    var viewModel = await sender.Send(query);

    return Results.Ok(viewModel);
});

await app.RunAsync();