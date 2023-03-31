using Movies.Domain.Common;

namespace Movies.Infrastructure.Repositories.Interfaces;

public interface IRepository < T > where T: BaseEntity
{
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    Task<PaginatedResponse<T>> GetPaginated(PaginatedRequest request, CancellationToken cancellationToken);
    Task<T?> Get(Guid id);  
    Task Insert(T entity, CancellationToken cancellationToken);  
    void Update(T entity);  
    void Delete(T entity);  
}  