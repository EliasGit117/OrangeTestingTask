using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Infrastructure.Persistance;
using Movies.Infrastructure.Repositories.Interfaces;

namespace Movies.Infrastructure.Repositories;

public class UserRepository: GenericRepository<User>, IUserRepository
{
    private DbSet<User> _users;

    public UserRepository(ApiDbContext context) : base(context)
    {
        _users = context.Set<User>();;
    }

    public async Task<User?> GetByName(string name, CancellationToken cancellationToken = default)
    {
        return await _users.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
    
    public async Task<bool> CheckNameUniqueness(string name, CancellationToken cancellationToken = default)
    {
        return await _users.AnyAsync(x => x.Name == name, cancellationToken);
    }
}