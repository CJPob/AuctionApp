namespace AuctionApp.Core.Interfaces;

/// <summary>
/// Defines the contract for persistence-related operations with auctions,
/// responsible for interacting with the database to manage auction data.
/// </summary>
public interface IAuctionPersistence
{
    List<Auction> GetActiveAuctions();
    
    Auction GetAuctionById(Guid id);
    
    List<Auction> GetAuctionByUserBids(string userName);

    void SaveAuction(Auction auction);
    
    void SaveBid(Bid bid);

    void EditDescription(Auction auction);
    
    List<Auction> GetAuctionByUserName(string userName);

    List<Auction> GetAuctionUserHasWon(string userName);
}