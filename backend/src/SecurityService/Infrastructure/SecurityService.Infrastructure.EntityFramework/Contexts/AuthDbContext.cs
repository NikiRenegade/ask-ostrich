using Microsoft.EntityFrameworkCore;
using SecurityService.Domain.Entities;
using SecurityService.Infrastructure.EntityFramework.Configurations;

namespace SecurityService.Infrastructure.EntityFramework.Contexts;

public class AuthDbContext
    : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
    }
}
