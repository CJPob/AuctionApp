namespace AuctionApp.Core;

public class UserDto
{
    public string Id { get; set; }       // User Id (mapped from AppIdentityUser db object from AspNetUsers table)
    public string UserName { get; set; } // UserName (mapped from AppIdentityUser db object from AspNetUsers table)
    public string Email { get; set; }    // Email (mapped from AppIdentityUser db object from  AspNetUsers table)
    public List<string> Roles { get; set; }  // Roles (mapped from AppIdentityUser db object from AspNetRoles via AspNetUserRoles)
}
