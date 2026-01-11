using IdentityService.Domain.Entities;

namespace IdentityService.Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
