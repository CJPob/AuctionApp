namespace AuctionApp.Core.Interfaces;

/// <summary>
/// Defines the contract for auction-related operations, serving as the bridge 
/// between the data model and the user interface in the auction system.
/// </summary>
public interface IAuctionService
{
    void CreateAuction(string userName, string title, string description, decimal openingBid, DateTime expirationDate);

    void PlaceBid(string userName, decimal bidAmount, Guid auctionId);

    List<Auction> GetActiveAuctions();
    
    List<Auction> GetAuctionByUserBids(string userName);

    List<Auction> GetAuctionByUserName(string userName);
    
    Auction GetAuctionDetails(Guid id);
    
    void EditDescription(string userName, string description, Guid auctionId);
    List<Auction> GetAuctionUserHasWon(string userName);

}