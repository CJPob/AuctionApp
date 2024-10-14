using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auction;

public class BidVm
{
    [ScaffoldColumn(false)]
    public Guid BidId { get; private set; }
    
    [Display(Name = "Placed by")]
    public string User { get; private set; }
    
    [Display(Name = "Bid amount")]
    public decimal BidAmount { get; private set; }
    
    [Display(Name = "Bid date")]
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