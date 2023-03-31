using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Infrastructure.Persistance;
using Movies.Infrastructure.Repositories.Interfaces;

namespace Movies.Infrastructure.Repositories;

public class FavoriteMoviesRepository : GenericRepository<FavoriteMovie>, IFavoriteMoviesRepository
{
    private readonly ApiDbContext _context;
    private readonly DbSet<FavoriteMovie> _favoriteMovies;

    public async Task<IEnumerable<FavoriteMovie>> GetFavMoviesByUserId
    (
        Guid userId, 
        CancellationToken cancellationToken = default(CancellationToken)
    )
    {
        return await _favoriteMovies.Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public FavoriteMoviesRepository(ApiDbContext context) : base(context)
    {
        _favoriteMovies = context.Set<FavoriteMovie>();
        _context = context;
    }

    public async Task<bool> CheckIfExists(Guid userId, int movieId, CancellationToken cancellationToken = default)
    {
        return await _favoriteMovies
            .AnyAsync(x => x.InternalId == movieId && x.UserId == userId, cancellationToken);
    }

    public async Task SetFavMovie(int id, Guid userId, bool isFav, CancellationToken cancellationToken = default)
    {
        var movie = await _favoriteMovies
            .Where(x => x.InternalId == id && x.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        switch (isFav)
        {
            case true when movie == null:
                await _favoriteMovies.AddAsync(new FavoriteMovie()
                {
                    UserId = userId,
                    InternalId = id
                }, cancellationToken);
                break;
            case false when movie != null:
                _favoriteMovies.Remove(movie);
                break;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}