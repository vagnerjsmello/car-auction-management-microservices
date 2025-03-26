using CAMS.Common.ResponseApi;
using MediatR;

namespace CAMS.Auctions.Application.Commands.Auctions.PlaceBid;

/// <summary>
/// Command to place a bid on an active auction.
/// </summary>
public class PlaceBidCommand : IRequest<ResponseResult<PlaceBidResponse>>
{
    public Guid AuctionId { get; }
    public decimal BidAmount { get; }
    public Guid BidderId { get; }

    public PlaceBidCommand(PlaceBidRequest request)
    {
        AuctionId = request.AuctionId;
        BidAmount = request.BidAmount;
        BidderId = request.BidderId;
    }
}
