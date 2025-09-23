using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService.Domain.Entities;

namespace SecurityService.Infrastructure.EntityFramework.Configurations;

internal sealed class UserConfiguration
    : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.PhoneNumber)
            .IsRequired(false)
            .HasMaxLength(25);

        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.PhoneNumber).IsUnique();
    }
}