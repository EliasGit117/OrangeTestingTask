using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Movies.API.Common.Jwt;
using Movies.Application.Movies.Queries;
using Movies.Application.Users;
using Movies.Application.Users.Commands;
using Movies.Domain.Common;
using Movies.Domain.Entities;
using Movies.Infrastructure.Repositories.Interfaces;
using Movies.Infrastructure.Services;
using Movies.Tests;

namespace Movie.Test;

public class Tests
{
    private static readonly string _url = "https://api.themoviedb.org/3/";
    private static readonly string _apiKey = "dad8a59d86a2793dda93aa485f7339c1";

    private static readonly Mock<IFavoriteMoviesRepository> _favMoviesRepo = new();
    private static readonly Mock<IUserRepository> _userRepo = new();

    private static IConfiguration configStub { get; set; }
    private static HttpClient _httpClient;

    static readonly Dictionary<string, string> config = new()
    {
        ["ApiKey"] = "",
    };

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_url);

        configStub = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        var mocks = ConfigConsts.GenereateMocks();

        _userRepo
            .Setup(x => x.GetAll(default))
            .ReturnsAsync(mocks.Users);

        _favMoviesRepo
            .Setup(x => x.GetAll(default))
            .ReturnsAsync(mocks.FavoriteMovies);
    }


    [Test]
    public async Task TestingMockData()
    {
        var users = (ICollection<User>)await _userRepo.Object.GetAll(default);
        var favMovies = (ICollection<FavoriteMovie>)await _favMoviesRepo.Object.GetAll(default);
        Assert.Multiple(() =>
        {
            Assert.That(users, Is.Not.Empty);
            foreach (var favMovie in favMovies)
            {
                Assert.That(users.Any(x => x.Id == favMovie.UserId));
            }
        });
    }

    [Test]
    public async Task TestAuth()
    {
        const string name = "SomeName117";
        const string password = "SomeName117";

        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>())
            .CreateMapper();

        // Creating new user
        var createUserCmd = new CreateUserCmd() { Name = name, Password = password };
        var createUserCmdHandler = new CreateUserCmdHandler(mapper, _userRepo.Object);
        await createUserCmdHandler.Handle(createUserCmd, default);
    }

    [Test]
    public void TestJwt()
    {
        const string key = "dad8a59d86a2793dda93aa485f7339c1";
        var param = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.FromMinutes(5),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateTokenReplay = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
        };

        var token = TokenGenerator
            .Create(new User() { Name = "S0ME_USER11", Password = "S0ME_PASS11" }, key);
        var claim = new JwtSecurityTokenHandler().ValidateToken(token, param, out _);
        
        Assert.NotNull(claim);
    }

    [Test]
    public async Task GetMovieDetails()
    {
        var getMovieQr = new GetMovieByIdQr()
        {
            Id = ConfigConsts.MoviesIds[
                new Random().Next(ConfigConsts.MoviesIds.Count)
            ]
        };

        var movieClient = new MovieDbClient
        (
            _httpClient, Options.Create(new MovieApiSettings() { ApiKey = _apiKey, Url = _url })
        );
        var getMovieQrHandler = new GetMovieByIdQrHandler(movieClient, _favMoviesRepo.Object);
        var movie = await getMovieQrHandler.Handle(getMovieQr, default);
        Assert.That(movie, Is.Not.Null);
    }
}