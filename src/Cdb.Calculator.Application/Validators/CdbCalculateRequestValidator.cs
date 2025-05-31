using Cdb.Calculator.Application.Dtos.Requests;
using FluentValidation;

namespace Cdb.Calculator.Application.Validators;

public class CdbCalculateRequestValidator : AbstractValidator<CdbCalculateRequest>
{
    public CdbCalculateRequestValidator()
    {
        RuleFor(x => x.InitialValue)
            .NotNull()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Months)
            .NotNull()
            .NotEmpty()
            .GreaterThan(1);
    }
}