using Cdb.Calculator.Application.Dtos.Requests;
using Cdb.Calculator.Application.Validators;
using FluentValidation.TestHelper;

namespace Cdb.Calculator.Application.Tests.Validators;

public class CdbCalculateRequestValidatorTests
{
    private readonly CdbCalculateRequestValidator _validator;

    public CdbCalculateRequestValidatorTests()
    {
        _validator = new CdbCalculateRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_InitialValue_Is_Less_Than_Zero()
    {
        var request = new CdbCalculateRequest(
            InitialValue: -1,
            Months: 10
        );

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.InitialValue);
    }

    [Fact]
    public void Should_Have_Error_When_Months_Is_Less_Than_Or_Equal_To_One()
    {
        var requestZero = new CdbCalculateRequest(
            InitialValue: 100,
            Months: 0
        );
        var requestOne = new CdbCalculateRequest(
            InitialValue: 100,
            Months: 1
        );

        _validator.TestValidate(requestZero).ShouldHaveValidationErrorFor(x => x.Months);
        _validator.TestValidate(requestOne).ShouldHaveValidationErrorFor(x => x.Months);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Request()
    {
        var request = new CdbCalculateRequest(
            InitialValue: 100,
            Months: 12
        );

        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}