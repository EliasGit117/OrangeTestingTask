using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Common.Jwt;
using Movies.API.Filters;
using Movies.Application.Common.Exceptions;
using Movies.Application.Genre.Queries;
using Movies.Application.Movies.Commands;
using Movies.Application.Movies.Queries;
using Movies.Application.Users.Commands;
using Movies.Domain.Common;
using Movies.Domain.Dtos;

namespace Movies.API.Controllers;

[ApiController]
[ApiExceptionFilter]
[Route("movies")]
public class MovieController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovieController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovieDetails(int id, CancellationToken cancellationToken)
    {
        var userId = User?.FindFirstValue(ClaimType.Id);

        var result = await _mediator.Send(new GetMovieByIdQr()
        {
            UserId = userId != null ? new Guid(userId) : null,
            Id = id
        }, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("genres")]
    public async Task<ActionResult<GenresDto>> GetGenres()
    {
        return await _mediator.Send(new GetGenresCmd());
    }

    [HttpGet("discover")]
    public async Task<ActionResult<PaginatedResponse<MovieDto>>> DiscoverMovies
    (
        [FromQuery] GetMoviesByGenreQr request,
        CancellationToken cancellationToken
    )
    {
        var a = 1;
        return await _mediator.Send(request, cancellationToken);
    }


    [HttpPut, Authorize]
    public async Task<ActionResult> SetFavorite([Required] int movieId, [Required] bool isFav,
        CancellationToken cancellationToken)
    {
        var id = User?.FindFirstValue(ClaimType.Id);

        await _mediator.Send
        (
            new SetFavMovieCmd
            {
                UserId = new Guid(id),
                IsFav = isFav,
                MovieId = movieId
            },
            cancellationToken
        );
        return Ok();
    }

    [HttpGet("fav")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetFavMovies()
    {
        var id = User?.FindFirstValue(ClaimType.Id);
        if (id == null)
            throw new BadRequestException("Not found id inside token!");

        return Ok
        (
            await _mediator.Send(new GetFavMoviesQr() { UserId = new Guid(id) })
        );
    }

    [HttpGet("filter")]
    public async Task<ActionResult<PaginatedResponse<MovieDto>>> GetFilteredMovies([FromQuery] GetMovieListQr request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}