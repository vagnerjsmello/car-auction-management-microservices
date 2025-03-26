using CAMS.Auctions.Domain.Entities;
using CAMS.Auctions.Domain.Enums;

namespace CAMS.Auctions.Application.Queries.Auctions.GetAuction;

/// <summary>
/// Response containing auction details.
/// </summary>
public class GetAuctionResponse
{
    public Guid AuctionId { get; }
    public Guid VehicleId { get; }
    public decimal HighestBid { get; }
    public AuctionStatus Status { get; }
    public List<Bid> Bids { get; set; }

    public GetAuctionResponse(Guid auctionId, Guid vehicleId, decimal highestBid, AuctionStatus status, List<Bid> bids)
    {
        AuctionId = auctionId;
        VehicleId = vehicleId;
        HighestBid = highestBid;
        Status = status;
        Bids = bids;
    }
}
