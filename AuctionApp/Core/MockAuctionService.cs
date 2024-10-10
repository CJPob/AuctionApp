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

        public Auction GetAuctionById(Guid id)
        {
            return _auctions.Find(a => a.Id == id);
        }
        
        private static readonly List<Auction> _auctions = new List<Auction>();
        static MockAuctionService()
        {
            Auction auction1 = new Auction("Vintage Car", "A rare vintage car", "user@test.com", 5000m, DateTime.Now.AddDays(10));
            Auction auction2 = new Auction("Painting", "Beautiful landscape painting", "user@test.se", 2000m, DateTime.Now.AddDays(5));

            // Användare 2 lägger ett bud på Auktion 1 (Vintage Car)
            Bid bid1 = new Bid("user@test.se", 5100m);
            auction1.AddBid("user@test.se", bid1); // Lägg bud för user2 på auction1

            // Användare 1 lägger ett bud på Auktion 2 (Painting)
            Bid bid2 = new Bid("user@test.com", 2100m);
            auction2.AddBid("user@test.com", bid2); // Lägg bud för user1 på auction2

            // Lägg till auktionerna i listan över auktioner
            _auctions.Add(auction1);
            _auctions.Add(auction2);
        }
    }
}