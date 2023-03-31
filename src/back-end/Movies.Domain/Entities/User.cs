using System.ComponentModel.DataAnnotations;

namespace Movies.Domain.Entities;

public class User: BaseEntity
{
    public string Name { get; set; }
    public string Password { get; set; }
    
    public List<FavoriteMovie> FavoriteMovies { get; set; }
}