using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Persistence;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }
    
    // DB set frågr till databasen
    public DbSet<AuctionDb> Auctions { get; set; }
    public DbSet<BidDb> Bids { get; set; }

}