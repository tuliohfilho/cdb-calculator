namespace Cdb.Calculator.Application.Dtos.Responses;

public record CdbCalculateResponse(
     decimal NetResult,
     decimal GrossResult
);