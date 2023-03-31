using MediatR;
using Movies.Domain.Dtos;
using Movies.Infrastructure.Repositories;
using Movies.Infrastructure.Repositories.Interfaces;
using Movies.Infrastructure.Services;

namespace Movies.Application.Movies.Queries;

public class GetFavMoviesQr: IRequest<IEnumerable<MovieDetailsDto?>>
{
    public Guid UserId { get; set; } 
}

public class GetFavMoviesQrHandler : IRequestHandler<GetFavMoviesQr, IEnumerable<MovieDetailsDto?>>
{
    private readonly MovieDbClient _movieDbClient;
    private readonly IFavoriteMoviesRepository _favoriteMoviesRepository;

    public GetFavMoviesQrHandler(MovieDbClient movieDbClient, IFavoriteMoviesRepository favoriteMoviesRepository)
    {
        _movieDbClient = movieDbClient;
        _favoriteMoviesRepository = favoriteMoviesRepository;
    }

    public async Task<IEnumerable<MovieDetailsDto?>> Handle(GetFavMoviesQr request, CancellationToken cancellationToken)
    {
        var favorites = await _favoriteMoviesRepository
            .GetFavMoviesByUserId(request.UserId, cancellationToken);

        var movieQueries = favorites
            .Select(favoriteMovie => _movieDbClient.GetMovie(favoriteMovie.InternalId, cancellationToken)).ToList();

        return await Task.WhenAll(movieQueries);
    }
}