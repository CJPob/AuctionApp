using System.ComponentModel.DataAnnotations;

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
    
    private DateTime _bidDate;

    public Bid(decimal bidAmount)
    {
        BidId = Guid.NewGuid();
        BidAmount = bidAmount;
        _bidDate = DateTime.Now;
    }
    
    public Bid() {}

    public override string ToString()
    {
        return $"{BidId}: {User}: {BidAmount}: {_bidDate}: ";
    }
}