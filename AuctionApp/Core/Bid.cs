namespace AuctionApp.Core;

/// <summary>
/// Represents a bid in an auction, containing information about the user who placed the bid, 
/// the bid amount, and the time the bid was made.
/// </summary>
public class Bid
{
    public Guid BidId { get; private set; }

    public string User { get; private set; }
    
    public decimal BidAmount { get; private set; }
    
    public DateTime BidDate { get; private set; }
    
    public Guid AuctionId { get; private set; }

    public Bid(string user, decimal bidAmount, Guid auctionId)
    {
        BidId = Guid.NewGuid();
        User = user;
        BidAmount = bidAmount;
        BidDate = DateTime.Now;
        AuctionId = auctionId;
    }
    
    public Bid() {}

    public override string ToString()
    {
        return $"{BidId}: {User}: {BidAmount}: {BidDate}: {AuctionId}";
    }
}