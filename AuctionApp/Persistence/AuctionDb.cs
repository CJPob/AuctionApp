using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Persistence
{
    public class AuctionDb
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string? Description { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Opening bid minimum is 1")]
        public decimal OpeningBid { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpirationDate { get; set; }

        public List<BidDb> Bids { get; set; } = new List<BidDb>();
    }
}