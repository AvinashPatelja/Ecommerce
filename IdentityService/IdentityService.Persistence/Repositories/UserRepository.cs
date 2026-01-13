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

    public async Task<List<User>> GetPendingUsersAsync()
    {
        return await _context.Users.Where(u => !u.IsApproved)
        .OrderBy(u => u.CreatedOn).ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
