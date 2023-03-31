using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Domain.Common;
using Movies.Domain.Entities;
using Movies.Infrastructure.Interceptors;
using Movies.Infrastructure.Persistance;
using Movies.Infrastructure.Repositories;
using Movies.Infrastructure.Repositories.Interfaces;
using Movies.Infrastructure.Services;

namespace Movies.Infrastructure;

using Microsoft.Extensions.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MovieApiSettings>(o => configuration.GetSection("MovieApiSettings").Bind(o));
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped(typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFavoriteMoviesRepository, FavoriteMoviesRepository>();

        services.AddHttpClient<MovieDbClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection("MovieApiSettings").GetValue<string>("Url")!);
        });
        
        services.AddDbContext<ApiDbContext>(
            opt => opt.UseNpgsql(configuration.GetConnectionString("Web"))
        );
        return services;
    }
}