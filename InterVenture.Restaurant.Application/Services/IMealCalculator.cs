namespace InterVenture.Restaurant.Application.Services;

public interface IMealCalculator
{
    double Calculate(double price);
}

internal sealed class MealCalculator : IMealCalculator
{
    public double Calculate(double price) => price * 0.9;
}