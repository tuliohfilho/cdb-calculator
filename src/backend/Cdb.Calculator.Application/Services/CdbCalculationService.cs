using Cdb.Calculator.Application.Dtos.Responses;
using Cdb.Calculator.Application.Extensions;
using Cdb.Calculator.Application.Interfaces.Services;

namespace Cdb.Calculator.Application.Services;

public class CdbCalculationService : ICdbCalculationService
{
    private const decimal CDI_RATE = 0.009m; // 0.9%
    private const decimal TB_RATE = 1.08m;  // 108%

    public CdbCalculateResponse Calculate(decimal initialValue, int months)
    {
        decimal currentGrossValue = initialValue;

        for (int i = 0; i < months; i++)
        {
            currentGrossValue *= (1 + (CDI_RATE * TB_RATE));
        }

        decimal profit = currentGrossValue - initialValue;
        decimal taxRate = GetTaxRate(months);
        decimal taxAmount = profit * taxRate;
        decimal netResult = currentGrossValue - taxAmount;

        decimal truncatedNetResult = netResult.Truncate(2);
        decimal truncatedGrossResult = currentGrossValue.Truncate(2);

        return new CdbCalculateResponse(truncatedNetResult, truncatedGrossResult);
    }

    private static decimal GetTaxRate(int months)
    {
        if (months <= 6)
            return 0.225m; // 22.5%
        else if (months <= 12)
            return 0.20m; // 20%
        else if (months <= 24)
            return 0.175m; // 17.5%
        else
            return 0.15m; // 15%
    }
}