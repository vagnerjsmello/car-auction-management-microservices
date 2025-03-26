namespace CAMS.Auctions.Application.Commands.Auctions.StartAuction;


/// <summary>
/// Response returned after starting an auction.
/// </summary>
public class StartAuctionResponse
{
    public Guid AuctionId { get; }
    public Guid VehicleId { get; }
    public Guid BidderId { get; }
    public decimal StartingBid { get; }


    public StartAuctionResponse(Guid auctionId, Guid vehicleId, Guid bidderId, decimal startingBid)
    {
        AuctionId = auctionId;
        VehicleId = vehicleId;
        BidderId = bidderId;
        StartingBid = startingBid;
    }
}