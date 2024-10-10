using System.ComponentModel.DataAnnotations;
using AuctionApp.Controllers;

namespace AuctionApp.Models.Auction;

public class AuctionDetailsVm
{
    [ScaffoldColumn(false)]
    public Guid Id { get; private set; }
    
    public string Name { get; set; }
    
    public string Desciption { get; set; }
    
    public string User { get; set; }
    
    [Display(Name = "Opening bid")]
    public decimal OpeningBid { get; private set; }
    
    [Display(Name = "Ends in")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime ExpirationDate { get; private set; }

    public List<BidVm> BidsVm { get; set; } = new();

    public static AuctionDetailsVm FromAuction(Core.Auction auction)
    {
        var detailsVM = new AuctionDetailsVm()
        {
            Id = auction.Id,
            Name = auction.Name,
            Desciption = auction.Description,
            User = auction.User,
            OpeningBid = auction.OpeningBid,
            ExpirationDate = auction.ExpirationDate
        };
        foreach (var bid in auction.Bids)
        {
            detailsVM.BidsVm.Add(BidVm.FromBid(bid));
        }
        return detailsVM;
    }
}