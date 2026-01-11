using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence.Repositories;

public class UserRepository
    : GenericRepository<User>, IUserRepository
{
    public UserRepository(IdentityDbContext context)
        : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}
