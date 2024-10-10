using AuctionApp.Core.Interfaces;

namespace AuctionApp.Core
{
    public class MockAuctionService : IAuctionService
    {

        public void CreateAuction(string userName, string title, string description, decimal openingBid, DateTime expirationDate)
        {
            Auction newAuction = new Auction(title, description, userName, openingBid, expirationDate);
            _auctions.Add(newAuction);
        }

        public List<Auction> GetActiveAuctions()
        {
            return _auctions
                .Where(a => a.ExpirationDate > DateTime.Now)
                .OrderBy(a => a.ExpirationDate)
                .ToList();
        }
        
        private static readonly List<Auction> _auctions = new List<Auction>();
        static MockAuctionService()
        {
            // Lägg till några fördefinierade auktioner för testning
            _auctions.Add(new Auction("Vintage Car", "A rare vintage car", "user@test.com", 5000m, DateTime.Now.AddDays(10)));
            _auctions.Add(new Auction("Painting", "Beautiful landscape painting", "user@test.com", 2000m, DateTime.Now.AddDays(5)));
        }
    }
}