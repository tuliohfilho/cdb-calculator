namespace Cdb.Calculator.Application.Extensions;

public static class DecimalExtensions
{
    public static decimal Truncate(this decimal value, int decimalPlaces)
    {
        if (decimalPlaces < 0)
            throw new ArgumentOutOfRangeException(nameof(decimalPlaces), "O número de casas decimais não pode ser negativo.");

        decimal factor = (decimal)Math.Pow(10, decimalPlaces);
        return Math.Truncate(value * factor) / factor;
    }
}