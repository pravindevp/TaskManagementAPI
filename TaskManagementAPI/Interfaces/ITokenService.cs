using TaskManagementAPI.Models;

namespace TaskManagementAPI.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(AppUser user);
}
