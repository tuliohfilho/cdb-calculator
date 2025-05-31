namespace Cdb.Calculator.Application.Dtos.Requests;

public record CdbCalculateRequest(
    decimal InitialValue,
    int Months
);