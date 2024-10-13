using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Persistence;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }
    
    // DB set frågr till databasen
    public DbSet<AuctionDb> Auctions { get; set; }
    public DbSet<BidDb> Bids { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AuctionDb auctionDb = new AuctionDb
        {
            Id = Guid.NewGuid(), // seed
            Name = "New Auction",
            Description = "New Auction description",
            User = "seed@kth.se",
            OpeningBid = 1,
            ExpirationDate = DateTime.Now.AddDays(7),
            Bids = new List<BidDb>()
        };
        modelBuilder.Entity<AuctionDb>().HasData(auctionDb);

        BidDb bidDb1 = new BidDb()
        {
            BidId = Guid.NewGuid(),
            User = "bidder@kth.se",
            BidAmount = 1m,
            BidDate = DateTime.Now,
            AuctionId = auctionDb.Id
        };
        BidDb bidDb2 = new BidDb()
        {
            BidId = Guid.NewGuid(),
            User = "bidder@kth.se",
            BidAmount = 3m,
            BidDate = DateTime.Now,
            AuctionId = auctionDb.Id
        };
        modelBuilder.Entity<BidDb>().HasData(bidDb1,bidDb2);
    }
}