using CAMS.Common.ResponseApi;
using MediatR;

namespace CAMS.Auctions.Application.Queries.Auctions.GetAuction;

/// <summary>
/// Query to retrieve details of a specific auction.
/// </summary>
public class GetAuctionQuery : IRequest<ResponseResult<GetAuctionResponse>>
{
    public Guid AuctionId { get; }

    public GetAuctionQuery(Guid auctionId)
    {
        AuctionId = auctionId;
    }
}
