using Microsoft.AspNetCore.Identity;

namespace SecurityService.Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } =
        new List<UserRole>();
}