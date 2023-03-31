namespace Movies.Domain.Dtos;

public class GenresDto
{
    public IEnumerable<GenreDto> Genres { get; set; }
}

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}