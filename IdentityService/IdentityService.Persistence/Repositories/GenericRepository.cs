using IdentityService.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly IdentityDbContext _context;

    public GenericRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id)
        => await _context.Set<T>().FindAsync(id);

    public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();

    public async Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
