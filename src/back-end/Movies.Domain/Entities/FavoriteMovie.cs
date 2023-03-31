namespace Movies.Domain.Entities;

public class FavoriteMovie: BaseEntity
{
    public virtual User User { get; set; }
    public Guid UserId { get; set; }
    public int InternalId { get; set; }
}