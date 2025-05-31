using Cdb.Calculator.Application.Dtos.Requests;
using Cdb.Calculator.Application.Dtos.Responses;
using Cdb.Calculator.Application.Interfaces.Services;
using MediatR;

namespace Cdb.Calculator.Application.Commands.Cdbs;

public record CdbCalculateCommand(CdbCalculateRequest Request) : IRequest<CdbCalculateResponse>;

public class CdbCalculateCommandHandler(
    ICdbCalculationService service
) : IRequestHandler<CdbCalculateCommand, CdbCalculateResponse>
{
    public async Task<CdbCalculateResponse> Handle(CdbCalculateCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var result = service.Calculate(
            request.InitialValue,
            request.Months
        );

        return await Task.FromResult(result);
    }
}