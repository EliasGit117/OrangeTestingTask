using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;

namespace Movies.Infrastructure.Configurations;

public static class FavoriteMovieConfiguration
{
    public static ModelBuilder ConfigureFavoriteMovies(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteMovie>()
            .HasOne(x => x.User)
            .WithMany(x => x.FavoriteMovies)
            .HasForeignKey(x => x.UserId);
        
        return modelBuilder;
    }
}