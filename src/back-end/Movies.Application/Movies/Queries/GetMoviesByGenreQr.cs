using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Movies.Domain.Common;
using Movies.Domain.Dtos;
using Movies.Infrastructure.Services;

namespace Movies.Application.Movies.Queries;

public class GetMoviesByGenreQr: IRequest<PaginatedResponse<MovieDto>>
{
    public int GenreId { get; set; }
}

public class GetMoviesByGenreQrHandler : IRequestHandler<GetMoviesByGenreQr, PaginatedResponse<MovieDto>>
{
    private readonly MovieDbClient _movieDbClient;

    public GetMoviesByGenreQrHandler(MovieDbClient movieDbClient)
    {
        _movieDbClient = movieDbClient;
    }

    public async Task<PaginatedResponse<MovieDto>> Handle(GetMoviesByGenreQr request, CancellationToken cancellationToken)
    {
        return await _movieDbClient.GetMoviesByGenre(request.GenreId, cancellationToken);
    }
}

public class GetMoviesByGenreQrValidator: AbstractValidator<GetMoviesByGenreQr>
{
    public GetMoviesByGenreQrValidator()
    {
        RuleFor(x => x.GenreId).NotNull();
    }
}