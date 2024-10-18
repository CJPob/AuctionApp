using AuctionApp.Areas.Identity.Data;
using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using AuctionApp.Data;
using AuctionApp.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Persistence;

public class UserPersistence : IUserPersistence
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;
    private readonly AuctionDbContext _auctionContext;
    private readonly AppIdentityDbContext _identityContext;
    private readonly IMapper _mapper;

    public UserPersistence(
        UserManager<AppIdentityUser> userManager,
        IAuctionRepository auctionRepository,
        AuctionDbContext auctionContext,
        AppIdentityDbContext identityContext,
        IMapper mapper, IBidRepository bidRepository)
    {
        _userManager = userManager;
        _auctionRepository = auctionRepository;
        _auctionContext = auctionContext;
        _identityContext = identityContext;
        _mapper = mapper;
        _bidRepository = bidRepository;
    }

    public List<UserDto> GetAllUsers()
    {
        var users = _userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = _userManager.GetRolesAsync(user).Result; // fetch roles for each user
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToList();
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public bool DeleteUser(Guid id)
    {
        using (var identityTransaction = _identityContext.Database.BeginTransaction())
        using (var auctionTransaction = _auctionContext.Database.BeginTransaction())
        {
            try
            {
                // Find the user by ID
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id.ToString());
                if (user == null)
                    return false;

                // Get all auctions related to the user
                var auctions = _auctionRepository.FindBy(a => a.User == user.UserName);
            
                // Remove each related auction (bids removed by FK on delete cascade)
                foreach (var auction in auctions)
                {
                    _auctionRepository.RemoveAuction(auction.Id);
                }
                _auctionRepository.Save();
                
                // Remove bids made by the user in any auctions
                var userBids = _bidRepository.FindBidsByUser(user.UserName);
                foreach (var bid in userBids)
                {
                    var bidDb = _bidRepository.GetById(bid.BidId);  // Use the existing GetById method
                    if (bidDb != null) {
                        _bidRepository.Delete(bidDb);
                    }
                }
                _bidRepository.Save();

                // Delete the user
                var result = _userManager.DeleteAsync(user).Result;
                if (!result.Succeeded)
                    return false;

                // Commit both transactions
                auctionTransaction.Commit();
                identityTransaction.Commit();

                return true;
            }
            catch
            {
                // Rollback both transactions in case of an error
                auctionTransaction.Rollback();
                identityTransaction.Rollback();
                return false;
            }
        }
    }
}