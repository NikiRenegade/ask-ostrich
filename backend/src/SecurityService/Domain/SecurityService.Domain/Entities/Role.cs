namespace SecurityService.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } =
        new List<UserRole>();
}
