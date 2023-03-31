using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Movies.Domain.Common;
using Movies.Domain.Dtos;

namespace Movies.Infrastructure.Services;

public class MovieDbClient
{
    private readonly string _apiKey;
    private readonly MovieApiSettings _movieApiSettings;

    private readonly HttpClient _httpClient;

    public MovieDbClient(HttpClient httpClient, IOptions<MovieApiSettings> settings)
    {
        _httpClient = httpClient;
        _movieApiSettings = settings.Value;
        _apiKey = settings.Value.ApiKey;
    }

    public async Task<MovieDetailsDto?> GetMovie
    (
        int id = 1,
        CancellationToken cancellationToken = default,
        string? language = "en-US"
    )
    {
        MovieDetailsDto? movie = null;
        var response = await _httpClient
            .GetAsync($"movie/{id}?api_key={_apiKey}&language={language}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            movie = await response.Content.ReadFromJsonAsync<MovieDetailsDto>(cancellationToken: cancellationToken);
        }

        return movie;
    }

    public async Task<GenresDto> GetGenreList
    (
        CancellationToken cancellationToken,
        string? language = "en-US"
    )
    {
        var response = await _httpClient
            .GetAsync($"genre/movie/list?api_key={_apiKey}&language={language}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return (await response.Content
                .ReadFromJsonAsync<GenresDto>(cancellationToken: cancellationToken))!;
        }

        return new GenresDto() {};
    }

    public async Task<PaginatedResponse<MovieDto>> GetMovieList
    (
        string searchType,
        CancellationToken cancellationToken = default,
        int? page = 1,
        string? language = "en-US"
    )
    {
        PaginatedResponse<MovieDto>? movies = null;
        var response = await _httpClient
            .GetAsync($"movie/{searchType}?api_key={_apiKey}&language={language}&page={page}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            movies = await response.Content
                .ReadFromJsonAsync<PaginatedResponse<MovieDto>>(cancellationToken: cancellationToken);
        }

        return movies ?? new PaginatedResponse<MovieDto>();
    }

    public async Task<PaginatedResponse<MovieDto>> GetMoviesByGenre
    (
        int genreId,
        CancellationToken cancellationToken = default,
        string? language = "en-US"
    )
    {
        PaginatedResponse<MovieDto>? movies = null;
        var response = await _httpClient
            .GetAsync($"discover/movie?with_genres={genreId}&api_key={_apiKey}&language={language}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            movies = await response.Content
                .ReadFromJsonAsync<PaginatedResponse<MovieDto>>(cancellationToken: cancellationToken);
        }

        return movies ?? new PaginatedResponse<MovieDto>();
    }
}