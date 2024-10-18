using AuctionApp.Core;

namespace AuctionApp.Persistence.Interfaces; 
public interface IBidRepository : IGenericRepository<BidDb>
{ 
    List<BidDb> FindBidsByAuctionId(Guid auctionId);
    List<Bid> FindBidsByUser(string userName);
    void AddBid(Bid bid);
    BidDb GetBidDbById(Guid bidId);
}