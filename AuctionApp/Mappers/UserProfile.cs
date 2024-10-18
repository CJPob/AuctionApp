using AuctionApp.Areas.Identity.Data;
using AuctionApp.Core;
using AuctionApp.Models.Auction;
using AutoMapper;

namespace AuctionApp.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
            CreateMap<AppIdentityUser, UserDto>();
            CreateMap<UserDto, UserVm>();
    }
}