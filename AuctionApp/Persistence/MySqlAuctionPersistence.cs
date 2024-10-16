using System.Data;
using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AuctionApp.Persistence;

public class MySqlAuctionPersistence : IAuctionPersistence
{
    private readonly AuctionDbContext _auctionDbContext;
    private readonly IMapper _mapper;

    public MySqlAuctionPersistence(AuctionDbContext auctionDbContext, IMapper mapper)
    {
        _auctionDbContext = auctionDbContext;
        _mapper = mapper;
    }
    
    public List<Auction> GetActiveAuctions()
    {
        return _auctionDbContext.Auctions
            .Where(a => a.ExpirationDate >= DateTime.Now)
            .Select(auctionDb => _mapper.Map<Auction>(auctionDb))
            .ToList(); // null/empty checks are in the controller 
    }

    public List<Auction> GetAuctionByUserName(string userName)
    {
        return _auctionDbContext.Auctions
            .Where(a => a.User == userName)
            .Select(auctionDb => _mapper.Map<Auction>(auctionDb))
            .ToList(); // null/empty checks are in the controller 
    }

    public Auction GetAuctionById(Guid id)
    {
        var auctionDb = _auctionDbContext.Auctions
            .Include(a => a.Bids)
            .FirstOrDefault(a => a.Id == id);

        if (auctionDb == null) throw new DataException("Auction not found.");

        return _mapper.Map<Auction>(auctionDb);
    }

    public List<Auction> GetAuctionByUserBids(string userName)
    {
        return _mapper.Map<List<Auction>>(
            _auctionDbContext.Auctions
                .Include(a => a.Bids)
                .Where(a => a.Bids.Any(b => b.User == userName))
                .ToList()
        );// null/empty checks are in the controller 
    }

    public void SaveAuction(Auction auction)
    {
        var auctionDb = _mapper.Map<AuctionDb>(auction);
        _auctionDbContext.Auctions.Add(auctionDb);
        _auctionDbContext.SaveChanges();
    }

    public void SaveBid(Bid bid)
    {
        var bidDb = _mapper.Map<BidDb>(bid);
        _auctionDbContext.Bids.Add(bidDb);
        _auctionDbContext.SaveChanges();
    }

    public void EditDescription(Auction auction)
    {
        var auctionDb = _auctionDbContext.Auctions.FirstOrDefault(a => a.Id == auction.Id);

        if (auctionDb == null)
            throw new DataException("Auction not found.");

        _mapper.Map(auction, auctionDb);
        _auctionDbContext.SaveChanges();
    }

    public List<Auction> GetAuctionUserHasWon(string userName)
    {
        return GetAuctionByUserBids(userName)
            .Where(a => a.ExpirationDate < DateTime.Now && 
                        a.Bids.Any(b => b.User == userName &&
                                        b.BidAmount == a.Bids.Max(bid => bid.BidAmount)))
            .ToList(); 
    } 
}
