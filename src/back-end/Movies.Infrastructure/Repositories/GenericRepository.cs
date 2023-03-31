using Microsoft.EntityFrameworkCore;
using Movies.Domain.Common;
using Movies.Infrastructure.Persistance;
using Movies.Infrastructure.Repositories.Interfaces;

namespace Movies.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApiDbContext _context;
    private DbSet<T> _entities;

    public GenericRepository(ApiDbContext context)
    {
        this._context = context;
        _entities = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _entities.ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResponse<T>> GetPaginated(PaginatedRequest request, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _entities.OrderBy(x => x.Created);
        var count = query.Count();

        query = query.Skip((request.Page - 1) * request.ItemsPerPage).Take(request.ItemsPerPage);

        var result = await query.ToListAsync(cancellationToken);
            
        return new PaginatedResponse<T>()
        {
            Results = result,
            Page = request.Page,
            TotalResults = count,
            TotalPages = (int)Math.Ceiling(count / (double)request.ItemsPerPage),
            ItemsPerPage = request.ItemsPerPage,
        };
    }

    public async Task<T?> Get(Guid id)
    { 
        return await _entities.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task Insert(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }

        await _entities.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }

        _context.SaveChanges();
    }

    public void Delete(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }

        _entities.Remove(entity);
        _context.SaveChanges();
    }
}