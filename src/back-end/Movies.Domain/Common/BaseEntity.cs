namespace Movies.Domain.Common;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}