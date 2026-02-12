using Application.Common.Abstractions;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<SuUser> _hasher = new();

    public string HashPassword(SuUser user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool VerifyPassword(SuUser user, string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result != PasswordVerificationResult.Failed;
    }
}
