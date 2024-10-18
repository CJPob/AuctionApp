using System.Data;
using AuctionApp.Core.Interfaces;
using AuctionApp.Persistence.Interfaces;

namespace AuctionApp.Core;

public class AuctionService : IAuctionService
{
    // 3
    // dependency injectione
    // private readonly IAuctionPersistence _auctionPersistence;
    
    // 4
    // dependency injections 
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository; 
    
    // 3 constructor 
    /*
    public AuctionService(IAuctionPersistence auctionPersistence)
    {
        auctionPersistence = _auctionPersistence; 
    } 
    */
    
    // 4 constructor 
    public AuctionService(IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
    }
    
    public void CreateAuction(string userName, string name, string description, decimal openingBid, DateTime expirationDate)
    {
        if (userName == null || name == null || openingBid < 1 || expirationDate < DateTime.Now) throw new DataException("Invalid auction creation");
        Auction auction = new Auction(name, description, userName, openingBid, expirationDate);
        //  _auctionPersistence.SaveAuction(auction);
        _auctionRepository.AddAuction(auction);
    }
    
    public void PlaceBid(string userName, decimal bidAmount, Guid auctionId)
    {
        try
        {
            // Auction auction = _auctionPersistence.GetAuctionById(auctionId);
            Auction auction = _auctionRepository.GetAuctionById(auctionId);
            if (auction == null)
                throw new InvalidOperationException("Auction not found.");
        
            Bid bid = new Bid(userName, bidAmount, auctionId);
            auction.AddBid(bid);
            //_auctionPersistence.SaveBid(bid);
            _bidRepository.AddBid(bid);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new InvalidOperationException($"Error placing bid: {ex.Message}", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException($"Error placing bid: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while placing the bid.", ex);
        }
    }

    public List<Auction> GetActiveAuctions()
    {
        List<Auction> auctions = _auctionRepository.GetActiveAuctions();
        return auctions;
    }
    
    public Auction GetAuctionDetails(Guid id)
    {
        Auction auction = _auctionRepository.GetAuctionById(id);
        return auction;
    }

    public List<Auction> GetAuctionByUserBids(string userName)
    {
        List<Auction> auctions = _auctionRepository.GetAuctionByUserBids(userName);
        return auctions; 
    }

    public void EditDescription(string userName, string description, Guid auctionId)
    {
        Auction auction = _auctionRepository.GetAuctionById(auctionId);
    
        if (auction == null)
        {
            throw new DataException("Auction not found");
        }

        auction.Description = description;

        _auctionRepository.EditDescription(auction);
    }

    public List<Auction> GetAuctionsByUserName(string userName)
    {
        List<Auction> auctions = _auctionRepository.GetAuctionsByUserName(userName);
        return auctions;
    }

    public List<Auction> GetAuctionUserHasWon(string userName)
    {
        List<Auction> auctions = _auctionRepository.GetAuctionUserHasWon(userName);
        return auctions;
    }    
    
    public bool RemoveAuction(Guid auctionId)
    {
        return _auctionRepository.RemoveAuction(auctionId); 
    }
}