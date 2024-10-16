using System.Data;
using AuctionApp.Core;
using AuctionApp.Persistence.Interfaces;
using AutoMapper; 

namespace AuctionApp.Persistence.Repositories;

public class AuctionRepository : GenericRepository<AuctionDb>, IAuctionRepository
{
    
    private readonly IMapper _mapper;
    private readonly IBidRepository _bidRepository; 
    
    
    public AuctionRepository(AuctionDbContext context, IMapper mapper, IBidRepository bidRepository) : base(context)
    {
        _mapper = mapper;
        _bidRepository = bidRepository;
    }

    public List<Auction> GetActiveAuctions()
    {
        return _mapper.Map<List<Auction>>(FindBy(a => a.ExpirationDate >= DateTime.Now).ToList());
    }

    public List<Auction> GetAuctionByUserName(string userName)
    {
        return _mapper.Map<List<Auction>>(FindBy(a => a.User == userName).ToList());
    }

    public Auction GetAuctionById(Guid id)
    {
        // get auc.db obj by id
        var auctionDb = GetById(id);
        if (auctionDb == null)
        {
            throw new DataException("Auction not found.");
        }
        // and list of corresponding bids 
        auctionDb.Bids = _bidRepository.FindBidsByAuctionId(id);
        // map auc.db onto core auc object 
        var auction = _mapper.Map<Auction>(auctionDb);
        
        return auction;
    }

    
    public List<Auction> GetAuctionByUserBids(string userName)
    {
        // get all bids user made (using the injected BidRepo) 
        var userBids = _bidRepository.FindBidsByUser(userName);
        // find corresponding auctions id (if several bids on same auc: we use distinct)
        var auctionIds = userBids.Select(b => b.AuctionId).Distinct();
        // get and map auc objects where auc.id == aucDB.id
        return _mapper.Map<List<Auction>>(FindBy(auction => auctionIds.Contains(auction.Id)));
    }


    public List<Auction> GetAuctionUserHasWon(string userName)
    {
        // 1: get all auc objects where the user has placed bids using method above 
        var auctions = GetAuctionByUserBids(userName);

        // filter auctions to find where the user has the highest bid and the auction has expired
        return auctions.Where(a =>
            a.ExpirationDate < DateTime.Now &&
            a.Bids.Any(b => b.User == userName &&
                            b.BidAmount == a.Bids.Max(bid => bid.BidAmount))).ToList();
    }        

    public void EditDescription(Auction auction)
    {
        var auctionDb = GetById(auction.Id);
        if (auctionDb == null)
        {
            throw new DataException("Auction not found.");
        } 
        _mapper.Map(auction, auctionDb);
        Save();
    }

    public void AddAuction(Auction auction)
    {
        AuctionDb auctionDb = _mapper.Map<AuctionDb>(auction);
        Add(auctionDb);
        Save(); // commit 
    }
}
