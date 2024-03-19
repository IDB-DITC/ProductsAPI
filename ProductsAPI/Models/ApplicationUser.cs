
using Microsoft.AspNetCore.Identity;

namespace ProductsAPI.Models;

public class ApplicationUser : IdentityUser
{
    public IList<string> Role { get; set; } = [];
}

public class UserRoleDto 
{
    public string Id { get; set; }
    public string Name { get; set; }
}