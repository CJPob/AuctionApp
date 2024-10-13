using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auction;

public class CreateAuctionVm
{
    [Required]
    [StringLength(64, ErrorMessage = "Max 64 character length")]
    [Display(Name = "Auction name")]  
    public string AuctionName { get; set; }
    
    [StringLength(256, ErrorMessage = "Max 256 character length")]
    [Display(Name = "Auction description")]
    public string? AuctionDescription { get; set; }
    
    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Opening bid must be at least 1")]
    [Display(Name = "Opening bid")]
    public decimal OpeningBid { get; set; }
    
    [Display(Name = "Ends in")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime ExpirationDate { get; set; }
}