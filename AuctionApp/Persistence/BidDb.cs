using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionApp.Persistence
{
    public class BidDb
    {
        [Key]
        public Guid BidId { get; set; }

        [Required]
        [MaxLength(64)]
        public string User { get; set; }
        
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Opening bid minimum is 1")]
        public decimal BidAmount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BidDate { get; set; }


        [ForeignKey("AuctionId")]
        public AuctionDb AuctionDb { get; set; }
        
        
        public Guid AuctionId { get; set; }
    }
}