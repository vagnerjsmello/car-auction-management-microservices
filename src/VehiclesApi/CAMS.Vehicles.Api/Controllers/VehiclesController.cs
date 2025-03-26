using CAMS.Common.ResponseApi;
using CAMS.Vehicles.Application.Commands.Vehicles.CreateVehicle;
using CAMS.Vehicles.Application.Queries.Vehicles.SearchVehicle;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CAMS.Vehicles.Api.Controllers;

[ApiController]
[Route("api/vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseResult<CreateVehicleResponse>), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
    {
        var result = await _mediator.Send(new CreateVehicleCommand(request));
        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetVehicleById), new { id = result.Data.Id }, result.Data);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SearchVehiclesResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetVehicles([FromQuery] SearchVehiclesRequest request)
    {
        var result = await _mediator.Send(new SearchVehiclesQuery(request));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CreateVehicleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetVehicleById(Guid id)
    {
        var query = new SearchVehiclesQuery(new SearchVehiclesRequest { VehicleId = id });
        var result = await _mediator.Send(query);
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
