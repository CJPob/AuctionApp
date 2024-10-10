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
            Auction auction1 = new Auction("Vintage Car", "A rare vintage car", "user@test.com", 5000m, DateTime.Now.AddDays(2));
            Auction auction2 = new Auction("Painting", "Beautiful landscape painting", "user@test.se", 2000m, DateTime.Now.AddDays(5));

            Bid bid1 = new Bid("user2@test.com", 5100m);  // User2 places a bid
            Bid bid2 = new Bid("user3@test.com", 5500m);  // Higher bid
            Bid bid3 = new Bid("user4@test.com", 5000m);  // Equal to opening bid
            Bid bid4 = new Bid("user5@test.com", 5200m);  // Between previous two bids

            auction1.AddBid("user2@test.com", bid1); 
            auction1.AddBid("user3@test.com", bid2);
            auction1.AddBid("user4@test.com", bid3);
            auction1.AddBid("user5@test.com", bid4);

            Bid bid5 = new Bid("user1@test.com", 2100m);  // User1 places a bid
            Bid bid6 = new Bid("user2@test.com", 2200m);  // Higher bid
            Bid bid7 = new Bid("user3@test.com", 2050m);  // Lower bid
            Bid bid8 = new Bid("user4@test.com", 2300m);  // Highest bid

            auction2.AddBid("user1@test.com", bid5);
            auction2.AddBid("user2@test.com", bid6);
            auction2.AddBid("user3@test.com", bid7);
            auction2.AddBid("user4@test.com", bid8);

            _auctions.Add(auction1);
            _auctions.Add(auction2);
        }
    }
}