using Domain.Entities.SU;

namespace Application.Common.Abstractions;

public interface IPasswordHasher
{
    string HashPassword(SuUser user, string password);
    bool VerifyPassword(SuUser user, string hashedPassword, string providedPassword);
}
