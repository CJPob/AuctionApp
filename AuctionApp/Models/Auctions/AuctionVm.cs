using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auction;

public class AuctionVm
{
    [ScaffoldColumn(false)]
    public Guid Id { get; private set; }
    
    [Display(Name = "Item")]
    public string Name { get; set; }
    
    [Display(Name = "Description")]
    public string Desciption { get; set; }
    
    [Display(Name = "Placed by")]
    public string User { get; set; }
    
    [Display(Name = "Start bid")]
    public decimal OpeningBid { get; private set; }
    
    [Display(Name = "Ends in")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
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