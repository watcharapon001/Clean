namespace Application.Common.Abstractions;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string username, string role, string orgId);
}
