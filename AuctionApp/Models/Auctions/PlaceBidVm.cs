using System.ComponentModel;
using Microsoft.Build.Framework;

namespace AuctionApp.Models.Auction;

public class PlaceBidVm
{
    [Required]
    [DisplayName("Bid amount")]
    public decimal BidAmount { get; set; }
}