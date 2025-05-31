using Cdb.Calculator.Application.Commands.Cdbs;
using Cdb.Calculator.Application.Dtos.Requests;
using Cdb.Calculator.Application.Dtos.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cdb.Calculator.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CdbController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("calculate")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CdbCalculateResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CdbCalculateResponse>> CalculateAsync([FromBody] CdbCalculateRequest request)
    {
        var command = new CdbCalculateCommand(request);

        return Ok(await mediator.Send(command));
    }
}