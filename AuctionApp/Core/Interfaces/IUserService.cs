namespace AuctionApp.Core.Interfaces;

public interface IUserService  
{
    List<UserDto> GetAllUsers();
    bool DeleteUser(Guid id); 
}