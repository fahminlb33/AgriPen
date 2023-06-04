namespace AgriPen.Domain.Entities;

public interface IEntityModel
{
    DateTimeOffset UpdatedAt { get; set; }
    DateTimeOffset CreatedAt { get; set; }
}
