namespace AuctionApp.Core.Interfaces
{
    public interface IUserPersistence
    {
        List<UserDto> GetAllUsers();
        bool DeleteUser(Guid id);
    }
}