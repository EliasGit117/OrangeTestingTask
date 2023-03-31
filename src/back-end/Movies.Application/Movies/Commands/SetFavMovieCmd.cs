using MediatR;
using Movies.Application.Common.Exceptions;
using Movies.Infrastructure.Repositories;
using Movies.Infrastructure.Repositories.Interfaces;
using Movies.Infrastructure.Services;

namespace Movies.Application.Movies.Commands;

public class SetFavMovieCmd: IRequest
{
    public Guid UserId { get; set; }
    public int MovieId { get; set; }
    public bool IsFav { get; set; }
}

public class SetFavMovieCmdHandler : IRequestHandler<SetFavMovieCmd>
{
    private readonly IFavoriteMoviesRepository _favMovies;
    private readonly MovieDbClient _movieDbClient;

    public SetFavMovieCmdHandler(IFavoriteMoviesRepository favMovies, MovieDbClient movieDbClient)
    {
        _movieDbClient = movieDbClient;
        _favMovies = favMovies;
    }

    public async Task Handle(SetFavMovieCmd request, CancellationToken cancellationToken)
    {
        if (await _movieDbClient.GetMovie(request.MovieId, cancellationToken) == null)
            throw new NotFoundException("No movie found in API with such id");
        
        await _favMovies.SetFavMovie(request.MovieId, request.UserId, request.IsFav, cancellationToken);
    }
}