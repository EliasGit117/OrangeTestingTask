using Movies.Domain.Entities;
using Movies.Infrastructure.Persistance;

namespace Movies.Infrastructure.Repositories.Interfaces;

public interface IFavoriteMoviesRepository: IRepository<FavoriteMovie>
{
    public Task<IEnumerable<FavoriteMovie>> GetFavMoviesByUserId
    (
        Guid userId,
        CancellationToken cancellationToken = default(CancellationToken)
    );

    public Task<bool> CheckIfExists(Guid userId, int movieId, CancellationToken cancellationToken = default);

    public Task SetFavMovie(int id, Guid userId, bool isFav, CancellationToken cancellationToken = default);
}