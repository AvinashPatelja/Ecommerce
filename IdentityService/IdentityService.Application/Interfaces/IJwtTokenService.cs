using IdentityService.Domain.Entities;

namespace IdentityService.Application.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
