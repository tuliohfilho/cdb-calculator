using Cdb.Calculator.Api.Controllers;
using Cdb.Calculator.Api.Tests.Helper;
using Cdb.Calculator.Application.Commands.Cdbs;
using Cdb.Calculator.Application.Dtos.Requests;
using Cdb.Calculator.Application.Dtos.Responses;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Cdb.Calculator.Api.Tests.Controllers;

public class CdbControllerTests
{
    private readonly Mock<IMediator> _mediator;

    private readonly CdbController _controller;

    public CdbControllerTests()
    {
        _mediator = new Mock<IMediator>();

        _controller = new CdbController(
            _mediator.Object
        );
    }

    [Fact]
    public async Task CalculateAsync_ReturnsExpectedStatusCode()
    {
        var request = new CdbCalculateRequest(
            Months: 6,
            InitialValue: 100
        );

        var response = new CdbCalculateResponse(
            NetResult: 104.63m,
            GrossResult: 105.97m
        );

        _mediator.Setup(x => x.Send(
            It.IsAny<CdbCalculateCommand>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(response);

        var actionResult = await _controller.CalculateAsync(request);

        actionResult.Result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public void CalculateAsync_ValidRoute_ReturnsExpectedRoute()
    {
        const string expectedRoute = "api/[controller]/calculate";

        var route = ControllerHelper.GetRoute<CdbController>(
            nameof(CdbController.CalculateAsync)
        );

        route.Should().Be(expectedRoute);
    }
}
