using Microsoft.AspNetCore.Identity;

namespace SecurityService.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } =
        new List<UserRole>();

    public User(string email, string userName, string firstName, string lastName)
    {
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
    }

    public bool CheckPassword(string password, IPasswordHasher<User> hasher)
    {
        return hasher.VerifyHashedPassword(this, PasswordHash, password) == PasswordVerificationResult.Success;
    }

    public void ChangePassword(string currentPassword, string newPassword, IPasswordHasher<User> hasher)
    {
        if (!CheckPassword(currentPassword, hasher))
            throw new InvalidOperationException("Неверный текущий пароль.");

        PasswordHash = hasher.HashPassword(this, newPassword);
    }

    // Метод для внешней аутентификации (Google, etc.)
    public static User CreateOrUpdateFromExternalProvider(string email, string userName, string firstName, string lastName)
    {
        return new User(email, userName, firstName, lastName);
    }
}