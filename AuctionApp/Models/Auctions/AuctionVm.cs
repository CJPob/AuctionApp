using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auction;

public class AuctionVm
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

    public static AuctionVm FromAuction(Core.Auction auction)
    {
        return new AuctionVm()
        {
            Id = auction.Id,
            Name = auction.Name,
            Desciption = auction.Description,
            User = auction.User,
            OpeningBid = auction.OpeningBid,
            ExpirationDate = auction.ExpirationDate
        };
    }
}