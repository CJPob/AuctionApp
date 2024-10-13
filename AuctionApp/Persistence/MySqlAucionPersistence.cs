using System.Data;
using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AuctionApp.Persistence;

public class MySqlAucionPersistence : IAuctionPersistence

{
    private readonly AuctionDbContext _auctionDbContext;
    private readonly IMapper _mapper;

    public MySqlAucionPersistence(AuctionDbContext auctionDbContext, IMapper mapper)
    {
        _auctionDbContext = auctionDbContext;
        _mapper = mapper;
    }

    public List<Auction> GetActiveAuctions()
    {
        var auctionDbs = _auctionDbContext.Auctions.Where(a => a.ExpirationDate >= DateTime.Now).ToList();

        List<Auction> result = new List<Auction>();
        foreach (AuctionDb auctionDb in auctionDbs)
        {
            Auction auction = _mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }

        return result;
    }

    public List<Auction> GetAuctionByUserName(string userName)
    {
        var auctionDbs = _auctionDbContext.Auctions.Where(a => a.User == userName).ToList();
        List<Auction> result = new List<Auction>();

        foreach (AuctionDb auctionDb in auctionDbs)
        {
            Auction auction = _mapper.Map<Auction>(auctionDb);
            result.Add(auction);
        }

        return result;
    }

    public Auction GetAuctionById(Guid id)
    {
        AuctionDb auctionDb = _auctionDbContext.Auctions
            .Where(a => a.Id == id)
            .Include(a => a.Bids)
            .FirstOrDefault();

        if (auctionDb == null) throw new DataException("Auction could not be found");

        Auction auction = _mapper.Map<Auction>(auctionDb);

        return auction;
    }


    public List<Auction> GetAuctionByUserBids(string userName)
    {
        var auctionDb = _auctionDbContext.Auctions
            .Where(a => a.Bids.Any(b => b.User == userName))
            .Include(a => a.Bids)
            .ToList();

        if (auctionDb == null || !auctionDb.Any())
            throw new DataException("User has not placed any bids.");

        // Map the list of AuctionDb entities to Auction model using _mapper
        var auctions = _mapper.Map<List<Auction>>(auctionDb);

        return auctions;
    }

    public void SaveAuction(Auction auction)
    {
        AuctionDb auctionDb = _mapper.Map<AuctionDb>(auction);
        _auctionDbContext.Auctions.Add(auctionDb);
        _auctionDbContext.SaveChanges();
    }

    public void SaveBid(Bid bid)
    {
        BidDb bidDb = _mapper.Map<BidDb>(bid);
        _auctionDbContext.Bids.Add(bidDb);
        _auctionDbContext.SaveChanges();
    }

    public void EditDescription(Auction auction)
    {
        // Fetch the entity from the database
        AuctionDb auctionDb = _auctionDbContext.Auctions.FirstOrDefault(a => a.Id == auction.Id);

        if (auctionDb != null)
        {
            // Use AutoMapper to map the updated Auction object to the existing AuctionDb entity
            _mapper.Map(auction, auctionDb); // This maps the updated fields from 'auction' to 'auctionDb'

            // The entity is already being tracked, no need to call Update
            _auctionDbContext.SaveChanges(); // Save the changes
        }
    }

    public List<Auction> GetAuctionUserHasWon(string userName)
    {
        List<Auction> auctions = GetAuctionByUserBids(userName);

        var wonAuctions = auctions
            .Where(a => a.ExpirationDate < DateTime.Now && // The auction has expired
                        a.Bids.Any(b => b.User == userName &&
                                        b.BidAmount ==
                                        a.Bids.Max(bid => bid.BidAmount))) // The user's bid is the highest
            .ToList();

        return wonAuctions;
    }
}