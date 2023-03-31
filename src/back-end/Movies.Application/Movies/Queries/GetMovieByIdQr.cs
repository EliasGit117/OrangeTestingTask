using FluentValidation;
using MediatR;
using Movies.Domain.Common;
using Movies.Domain.Dtos;
using Movies.Infrastructure.Repositories;
using Movies.Infrastructure.Repositories.Interfaces;
using Movies.Infrastructure.Services;

namespace Movies.Application.Movies.Queries;

public class GetMovieByIdQr : IRequest<MovieDetailsDto?>
{
    public int Id { get; set; }
    public Guid? UserId { get; set; } 
}

public class GetMovieByIdQrHandler : IRequestHandler<GetMovieByIdQr, MovieDetailsDto?>
{
    private readonly MovieDbClient _movieDbClient;
    private readonly IFavoriteMoviesRepository _favoriteMoviesRepository;

    public GetMovieByIdQrHandler(MovieDbClient movieDbClient, IFavoriteMoviesRepository favoriteMoviesRepository)
    {
        _movieDbClient = movieDbClient;
        _favoriteMoviesRepository = favoriteMoviesRepository;
    }

    public async Task<MovieDetailsDto?> Handle(GetMovieByIdQr request, CancellationToken cancellationToken)
    {
        var movieRequest = _movieDbClient.GetMovie(id: request.Id, cancellationToken: cancellationToken);
        MovieDetailsDto? movie;
        
        if (request.UserId != null)
        {
            var isFavRequest = _favoriteMoviesRepository
                .CheckIfExists(request.UserId.Value, request.Id, cancellationToken);
            await Task.WhenAll(movieRequest, isFavRequest);

            movie = movieRequest.Result;
            movie!.IsFav = isFavRequest.Result;
        }
        else
        {
            return await movieRequest;
        }

        return movie;
    }
}

public class GetMovieByIdQrValidator : AbstractValidator<GetMovieByIdQr>
{
    public GetMovieByIdQrValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}