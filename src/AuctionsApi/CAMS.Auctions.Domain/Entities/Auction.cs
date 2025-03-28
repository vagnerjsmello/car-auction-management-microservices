﻿using CAMS.Auctions.Domain.Enums;
using CAMS.Auctions.Domain.Events;
using CAMS.Common.Entities;
using CAMS.Common.Exceptions;

namespace CAMS.Auctions.Domain.Entities;

/// <summary>
/// Represents an auction for a vehicle, tracking bids and managing auction state.
/// </summary>
public class Auction : AggregateRoot
{
    public Guid VehicleId { get; set; }
    public AuctionStatus Status { get; private set; }
    public decimal HighestBid { get; private set; }
    public List<Bid> Bids { get; private set; } = new List<Bid>();

    public Auction(Guid vehicleId, decimal startingBid) : base(Guid.NewGuid())
    {
        if (startingBid < 0)
            throw new ArgumentException("Starting bid must be non-negative.", nameof(startingBid));

        VehicleId = vehicleId;
        HighestBid = startingBid;
        Status = AuctionStatus.Active;

        // Record the auction started event.
        DomainEvents.Add(new AuctionStartedEvent(this.Id, vehicleId, startingBid));
    }

    public void PlaceBid(Bid bid)
    {
        if (Status != AuctionStatus.Active)
            throw InvalidBidException.AuctionNotActive();

        if (bid.Amount <= HighestBid)
            throw InvalidBidException.MustExceedCurrentBid();


        Bids.Add(bid);
        HighestBid = bid.Amount;

        // Record the bid placed event.
        DomainEvents.Add(new BidPlacedEvent(this.Id, VehicleId, bid.Amount, bid.BidderId, bid.Timestamp));
    }

    public void Close()
    {
        if (Status != AuctionStatus.Active)
            throw new InvalidOperationException("Auction is already closed.");

        Status = AuctionStatus.Closed;

        // Record the auction closed event.
        DomainEvents.Add(new AuctionClosedEvent(this.Id, VehicleId, HighestBid, DateTime.UtcNow));
    }
}
