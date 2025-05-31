using Cdb.Calculator.Application.Commands.Cdbs;
using Cdb.Calculator.Application.Dtos.Requests;
using Cdb.Calculator.Application.Dtos.Responses;
using Cdb.Calculator.Application.Interfaces.Services;
using FluentAssertions;
using Moq;

namespace Cdb.Calculator.Application.Tests.Commands.Cdbs;

public class CdbCalculateCommandTests
{
    private readonly Mock<ICdbCalculationService> _serviceMock;

    private readonly CdbCalculateCommandHandler _handler;

    public CdbCalculateCommandTests()
    {
        _serviceMock = new Mock<ICdbCalculationService>();

        _handler = new CdbCalculateCommandHandler(
            _serviceMock.Object
        );
    }

    [Fact]
    public void CdbCalculateCommand_Should_Set_Request_Property()
    {
        var request = GetRequest(200, 12);
        var command = new CdbCalculateCommand(request);

        command.Request.Should().Be(request);
    }

    [Fact]
    public void CdbCalculateCommand_Equality_Should_Work()
    {
        var request = GetRequest(300, 24);
        var command1 = new CdbCalculateCommand(request);
        var command2 = new CdbCalculateCommand(request);

        command1.Should().Be(command2);
    }

    private static CdbCalculateRequest GetRequest(decimal initialValue = 100, int months = 6) => 
        new(initialValue, months);

    [Fact]
    public async Task Handle_Should_Return_Correct_Response()
    {
        var request = GetRequest(100, 6);
        var expectedResponse = new CdbCalculateResponse(104.63m, 105.97m);

        _serviceMock.Setup(s => s.Calculate(request.InitialValue, request.Months))
            .Returns(expectedResponse);

        var command = new CdbCalculateCommand(request);

        var result = await _handler.Handle(command, default);

        result.Should().BeEquivalentTo(expectedResponse);
        _serviceMock.Verify(s => s.Calculate(request.InitialValue, request.Months), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Service_Throws()
    {
        var request = GetRequest(100, 6);
        var command = new CdbCalculateCommand(request);

        _serviceMock.Setup(s => s.Calculate(It.IsAny<decimal>(), It.IsAny<int>()))
            .Throws(new InvalidOperationException("Erro de cálculo"));

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Erro de cálculo");
    }
}