using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Infrastructure.Persistance;

namespace Movies.Infrastructure.Repositories.Interfaces;

public interface IUserRepository: IRepository<User>
{
    public Task<User?> GetByName(string name, CancellationToken cancellationToken = default);

    public Task<bool> CheckNameUniqueness(string name, CancellationToken cancellationToken = default);
}