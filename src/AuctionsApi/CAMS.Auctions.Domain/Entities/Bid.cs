namespace CAMS.Auctions.Domain.Entities;

/// <summary>
/// Represents a bid placed in an auction.
/// </summary>
public class Bid
{
    public decimal Amount { get; set; }
    public Guid BidderId { get; set; }
    public DateTime Timestamp { get; set; }

    public Bid() { }

    public Bid(decimal amount, Guid? bidderId)
    {
        Amount = amount;
        BidderId = bidderId.HasValue ? bidderId.Value : Guid.NewGuid(); //Simulate User Id
        Timestamp = DateTime.UtcNow;
    }    
}