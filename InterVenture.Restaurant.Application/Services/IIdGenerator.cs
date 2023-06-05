namespace InterVenture.Restaurant.Application.Services;

public interface IIdGenerator
{
    Guid New();
}

internal sealed class IdGenerator : IIdGenerator
{
    public Guid New() => Guid.NewGuid();
}