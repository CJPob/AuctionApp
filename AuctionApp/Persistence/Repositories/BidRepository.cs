using AuctionApp.Core;
using AuctionApp.Persistence.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Persistence.Repositories;

public class BidRepository : GenericRepository<BidDb>, IBidRepository
{
    private readonly IMapper _mapper;
    
    public BidRepository(AuctionDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public List<BidDb> FindBidsByAuctionId(Guid auctionId)
    {
        return FindBy(bid => bid.AuctionId == auctionId).ToList(); 
    }

    public List<Bid> FindBidsByUser(string userName)
    {
        return _mapper.Map<List<Bid>>(FindBy(bid => bid.User == userName).ToList()); 
    }  
    
    public void AddBid(Bid bid)
    {
        BidDb bidDb = _mapper.Map<BidDb>(bid);
        Add(bidDb);
        Save(); // commit 
    }
}