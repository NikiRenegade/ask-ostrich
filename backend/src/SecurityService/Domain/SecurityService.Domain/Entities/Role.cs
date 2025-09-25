using Microsoft.AspNetCore.Identity;

namespace SecurityService.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public virtual ICollection<UserRole> UserRoles { get; set; } =
        new List<UserRole>();
}
