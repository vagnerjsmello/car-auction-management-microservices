using CAMS.Common.ResponseApi;
using MediatR;

namespace CAMS.Auctions.Application.Commands.Auctions.CloseAuction;

/// <summary>
/// Command to close an active auction.
/// </summary>
public class CloseAuctionCommand : IRequest<ResponseResult<CloseAuctionResponse>>
{
    public Guid AuctionId { get; }

    public CloseAuctionCommand(CloseAuctionRequest request)
    {
        AuctionId = request.AuctionId;
    }
}
