using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auction;

public class UserVm
{
    [ScaffoldColumn(false)]
    public string Id { get; private set; }

    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Display(Name = "Email Address")]
    [EmailAddress]
    public string Email { get; set; }

    [Display(Name = "Roles")]
    public List<string> Roles { get; set; }
    
    public static UserVm FromUserDto(Core.UserDto userDto)
    {
        return new UserVm
        {
            Id = userDto.Id,
            UserName = userDto.UserName,
            Email = userDto.Email,
            Roles = userDto.Roles
        };
    }
}