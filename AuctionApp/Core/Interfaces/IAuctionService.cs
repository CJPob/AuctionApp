namespace AuctionApp.Core.Interfaces;

/// <summary>
/// Defines the contract for auction-related operations, serving as the bridge 
/// between the data model and the user interface in the auction system.
/// </summary>
public interface IAuctionService
{
    void CreateAuction(string userName, string title, string description, decimal openingBid, DateTime expirationDate);

    List<Auction> GetActiveAuctions();
    
    public Auction GetAuctionById(Guid id);
}