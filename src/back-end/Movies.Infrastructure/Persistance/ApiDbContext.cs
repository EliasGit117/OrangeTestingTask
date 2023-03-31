using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Infrastructure.Configurations;
using Movies.Infrastructure.Interceptors;

namespace Movies.Infrastructure.Persistance;

public class ApiDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public virtual DbSet<User?> Users { get; set; }

    public ApiDbContext
    (
        DbContextOptions<ApiDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
    ) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureUsers();
        modelBuilder.ConfigureFavoriteMovies();
    }
}