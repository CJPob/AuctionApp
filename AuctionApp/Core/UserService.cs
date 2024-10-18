using AuctionApp.Core.Interfaces;

namespace AuctionApp.Core
{
    public class UserService : IUserService
    {
        private readonly IUserPersistence _userPersistence;

        public UserService(IUserPersistence userPersistence)
        {
            _userPersistence = userPersistence;
        }

        public List<UserDto> GetAllUsers()
        {
            return _userPersistence.GetAllUsers();
        }

        public bool DeleteUser(Guid id)
        {
            return _userPersistence.DeleteUser(id); 
        }  
        
        
    }
}