using System.Data;

namespace AuctionApp.Core;

/// <summary>
/// Represents an auction that contains bids and information about the item being auctioned.
/// The auction remains active until the specified expiration date is reached.
/// </summary>
public class Auction
{
    // Properties and fields
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public string? Description { get; set; }
    
    public string User { get; private set; }
    
    public decimal OpeningBid { get; private set; }
    
    public DateTime ExpirationDate { get; private set; }
    
    private List<Bid> _bids = new List<Bid>();
    public IEnumerable<Bid> Bids => _bids;

    public Auction(string name, string description, string userName, decimal openingBid, DateTime expirationDate)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        User = userName;
        OpeningBid = openingBid;
        ExpirationDate = expirationDate;
    }
    
    public Auction() {}
    
    public void AddBid(Bid bid)
    {
        decimal highestBid = _bids.Any() ? _bids.Max(b => b.BidAmount) : OpeningBid;

        if (bid.User == User)
            throw new InvalidOperationException("Auction owner cannot place a bid on their own auction.");

        if (DateTime.Now > ExpirationDate)
            throw new InvalidOperationException("The auction has already expired.");

        if (bid.BidAmount <= highestBid)
            throw new ArgumentOutOfRangeException(nameof(bid.BidAmount), "Bid amount must be higher than the current highest bid.");

        _bids.Add(bid); 
    }

    public override string ToString()
    {
        return $"{Id}: {Name}: {User}: {Description}: {OpeningBid} ({ExpirationDate})";
    }
}