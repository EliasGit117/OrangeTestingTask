using FluentValidation;
using MediatR;
using Movies.Application.Common.Readonly;
using Movies.Domain.Common;
using Movies.Domain.Dtos;
using Movies.Infrastructure.Services;

namespace Movies.Application.Movies.Queries;

public class GetMovieListQr : PaginatedRequest, IRequest<PaginatedResponse<MovieDto>>
{
    public string SearchType { get; set; }
}

public class GetMovieListQrHandler : IRequestHandler<GetMovieListQr, PaginatedResponse<MovieDto>>
{
    private readonly MovieDbClient _movieDbClient;

    public GetMovieListQrHandler(MovieDbClient movieDbClient)
    {
        _movieDbClient = movieDbClient;
    }

    public async Task<PaginatedResponse<MovieDto>> Handle(GetMovieListQr request, CancellationToken cancellationToken)
    {
        return await _movieDbClient
            .GetMovieList(request.SearchType, page: request.Page, cancellationToken: cancellationToken);
    }
}

public class GetMovieListQrValidator : AbstractValidator<GetMovieListQr>
{
    public GetMovieListQrValidator()
    {
        RuleFor(x => x.SearchType)
            .Must(x => new[] { MovieSearchType.MostPopular, MovieSearchType.TopRated }.Contains(x)).WithMessage("Wrong type")
            .NotEmpty();
        RuleFor(x => x.Page).GreaterThan(0);
    }
}