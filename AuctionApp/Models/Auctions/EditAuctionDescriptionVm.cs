using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auction;

public class EditAuctionDescriptionVm
{
    [StringLength(256)]
    public string Description { get; set; }
}