using MediatR;
using Movies.Domain.Dtos;
using Movies.Infrastructure.Persistance;
using Movies.Infrastructure.Services;

namespace Movies.Application.Genre.Queries;

public class GetGenresCmd: IRequest<GenresDto> { }

public class GetGenresCmdHandler : IRequestHandler<GetGenresCmd, GenresDto>
{
    private readonly MovieDbClient _movieDbClient;

    public GetGenresCmdHandler(MovieDbClient movieDbClient)
    {
        _movieDbClient = movieDbClient;
    }

    public async Task<GenresDto> Handle(GetGenresCmd request, CancellationToken cancellationToken)
    {
        return await _movieDbClient.GetGenreList(cancellationToken);
    }
}