using FluentAssertions;
using Cdb.Calculator.Application.Extensions;

namespace Cdb.Calculator.Application.Tests.Extensions;

public class DecimalExtensionsTests
{
    [Theory]
    [InlineData(123.4567, 2, 123.45)]
    [InlineData(123.4567, 0, 123)]
    [InlineData(123.4567, 3, 123.456)]
    [InlineData(-123.4567, 2, -123.45)]
    [InlineData(1.9999, 2, 1.99)]
    [InlineData(0.0001, 2, 0.00)]
    [InlineData(100.0, 2, 100.0)]
    public void Truncate_Should_Truncate_To_Expected_Value(decimal value, int decimalPlaces, decimal expected)
    {
        var result = value.Truncate(decimalPlaces);

        result.Should().Be(expected);
    }

    [Fact]
    public void Truncate_Should_Throw_When_DecimalPlaces_Is_Negative()
    {
        Action act = () => 123.45m.Truncate(-1);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .Where(e => e.Message.Contains("O número de casas decimais não pode ser negativo"));
    }
}