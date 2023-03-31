using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;

namespace Movies.Infrastructure.Configurations;

public static class UserConfiguration
{
    public static ModelBuilder ConfigureUsers(this ModelBuilder modelBuilder)
    {
        return modelBuilder.Entity<User>(u =>
        {
            u.HasIndex(x => x.Name).IsUnique();
            u.Property(x => x.Name).HasMaxLength(100).IsRequired();
            u.Property(x => x.Password).HasMaxLength(100).IsRequired();
        });
    }
}