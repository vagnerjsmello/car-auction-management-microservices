using CAMS.Auctions.Application.Commands.Auctions.CloseAuction;
using CAMS.Auctions.Application.Commands.Auctions.PlaceBid;
using CAMS.Auctions.Application.Commands.Auctions.StartAuction;
using CAMS.Auctions.Application.Queries.Auctions.GetAuction;
using CAMS.Auctions.Application.Queries.Auctions.SearchAuctions;
using CAMS.Common.ResponseApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace CAMS.Auctions.Api.Controllers;




[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuctionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseResult<StartAuctionResponse>), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> StartAuction([FromBody] StartAuctionRequest request)
    {
        var command = new StartAuctionCommand(request);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetAuctionById), new { id = result.Data.AuctionId }, result.Data);
    }

    [HttpPost("{id}/bids")]
    [ProducesResponseType(typeof(ResponseResult<PlaceBidResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> PlaceBid(Guid id, [FromBody] PlaceBidRequest request)
    {
        request.AuctionId = id;
        var command = new PlaceBidCommand(request);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("{id}/close")]
    [ProducesResponseType(typeof(ResponseResult<CloseAuctionResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CloseAuction(Guid id)
    {
        var request = new CloseAuctionRequest { AuctionId = id };
        var result = await _mediator.Send(new CloseAuctionCommand(request));
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SearchAuctionsResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAuctions([FromQuery] SearchAuctionsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetAuctionResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAuctionById(Guid id)
    {
        var result = await _mediator.Send(new GetAuctionQuery(auctionId: id));
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}




