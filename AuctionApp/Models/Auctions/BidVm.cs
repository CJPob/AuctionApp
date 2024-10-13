using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auction;

public class BidVm
{
    [ScaffoldColumn(false)]
    public Guid BidId { get; private set; }
    
    public string User { get; private set; }
    
    public decimal BidAmount { get; private set; }
    
    public DateTime BidDate { get; private set; }

    public static BidVm FromBid(Core.Bid bid)
    {
        return new BidVm()
        {
            BidId = bid.BidId,
            User = bid.User,
            BidAmount = bid.BidAmount,
            BidDate = bid.BidDate
        };
    }
}