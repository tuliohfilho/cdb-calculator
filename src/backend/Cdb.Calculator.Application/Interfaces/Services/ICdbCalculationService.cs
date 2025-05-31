using Cdb.Calculator.Application.Dtos.Responses;

namespace Cdb.Calculator.Application.Interfaces.Services;

public interface ICdbCalculationService
{
    CdbCalculateResponse Calculate(decimal initialValue, int months);
}