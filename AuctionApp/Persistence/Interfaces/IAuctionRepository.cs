using AuctionApp.Core;

namespace AuctionApp.Persistence.Interfaces;
public interface IAuctionRepository : IGenericRepository<AuctionDb>
{
    List<Auction> GetActiveAuctions();
    List<Auction> GetAuctionsByUserName(string userName);
    Auction GetAuctionById(Guid id);
    List<Auction> GetAuctionByUserBids(string userName);
    List<Auction> GetAuctionUserHasWon(string userName);
    void EditDescription(Auction auction); 
    void AddAuction(Auction auction);
    bool RemoveAuction(Guid auctionId);
}
