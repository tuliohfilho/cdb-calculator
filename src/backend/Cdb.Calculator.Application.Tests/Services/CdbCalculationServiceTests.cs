using Cdb.Calculator.Application.Services;
using FluentAssertions;

namespace Cdb.Calculator.Application.Tests.Services;

public class CdbCalculationServiceTests
{
    private readonly CdbCalculationService _service;

    public CdbCalculationServiceTests()
    {
        _service = new CdbCalculationService();
    }

    [Theory]
    [InlineData(100, 6, 105.97, 104.63)]   // 22.5% taxa
    [InlineData(100, 12, 112.30, 109.84)]  // 20% taxa
    [InlineData(100, 24, 126.13, 121.55)]  // 17.5% taxa
    [InlineData(100, 36, 141.65, 135.4)]  // 15% taxa
    public void Calculate_Should_Return_Correct_Results(decimal initialValue, int months, decimal expectedGross, decimal expectedNet)
    {
        var result = _service.Calculate(initialValue, months);

        result.GrossResult.Should().Be(expectedGross);
        result.NetResult.Should().Be(expectedNet);
    }
}