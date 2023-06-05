namespace InterVenture.Restaurant.Application.Services;

public interface IDiscountCalculator
{
    double Calculate(double originalPrice, DateTimeOffset at, int restaurantOffset);
}

internal sealed class DiscountCalculator : IDiscountCalculator
{
    public double Calculate(double originalPrice, DateTimeOffset at, int restaurantOffset)
    {
        return IsHappyHour(at, restaurantOffset) ? originalPrice * 0.2 : 0;

        static bool IsHappyHour(DateTimeOffset at, int restaurantOffset)
        {
            var clientOffset = at.Offset.Hours;
            var offsetDiff = restaurantOffset - clientOffset;
            var happyHourCandidate = at.DateTime.AddHours(offsetDiff).Hour;

            return !(happyHourCandidate < 13 || happyHourCandidate > 15);
        }
    }
}