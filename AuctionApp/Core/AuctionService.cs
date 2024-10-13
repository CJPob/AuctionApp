using System.Data;
using AuctionApp.Core.Interfaces;

namespace AuctionApp.Core;

//parametrar från presentationslagret och omvandlar till 
public class AuctionService : IAuctionService
{
    private readonly IAuctionPersistence _auctionPersistence;
    
    public AuctionService(IAuctionPersistence auctionPersistence)
    {
        _auctionPersistence = auctionPersistence;
    }
    
    public void CreateAuction(string userName, string name, string description, decimal openingBid, DateTime expirationDate)
    {
        if (userName == null || name == null || openingBid < 1 || expirationDate < DateTime.Now) throw new DataException("Invalid auction creation");
        Auction auction = new Auction(name, description, userName, openingBid, expirationDate);
        _auctionPersistence.SaveAuction(auction);
    }
    
    public void PlaceBid(string userName, decimal bidAmount, Guid auctionId)
    {
        try
        {
            Auction auction = _auctionPersistence.GetAuctionById(auctionId);
            if (auction == null)
                throw new InvalidOperationException("Auction not found.");
        
            Bid bid = new Bid(userName, bidAmount, auctionId);
            auction.AddBid(bid);
            _auctionPersistence.SaveBid(bid);
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
        List<Auction> auctions = _auctionPersistence.GetActiveAuctions();
        return auctions;
    }
    
    public Auction GetAuctionDetails(Guid id)
    {
        Auction auction = _auctionPersistence.GetAuctionById(id);
        return auction;
    }

    public List<Auction> GetAuctionByUserBids(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetAuctionByUserBids(userName);
        return auctions; 
    }

    public void EditDescription(string userName, string description, Guid auctionId)
    {
        Auction auction = _auctionPersistence.GetAuctionById(auctionId);
    
        if (auction == null)
        {
            throw new DataException("Auction not found");
        }

        auction.Description = description;

        _auctionPersistence.EditDescription(auction);
    }

    public List<Auction> GetAuctionByUserName(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetAuctionByUserName(userName);
        return auctions;
    }

    public List<Auction> GetAuctionUserHasWon(string userName)
    {
        List<Auction> auctions = _auctionPersistence.GetAuctionUserHasWon(userName);
        return auctions;
    }
}