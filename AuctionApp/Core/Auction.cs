using System.ComponentModel.DataAnnotations;

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

    // Constructor
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

    
    //Business operations, // kanske behöver ta in en parameter till, vem är det som försöker lägga ett bud?
    public void AddBid(Bid bid)
    {
        // might need a validation as to see if the auction has ended or not. 
        // validation, ,only place bids that are higher than the opening bid
        // Man ska inte få placera bud på sin egna auction
        _bids.Add(bid);
    }

    public override string ToString()
    {
        return $"{Id}: {Name}: {User}: {Description}: {OpeningBid} ({ExpirationDate})";
    }
}