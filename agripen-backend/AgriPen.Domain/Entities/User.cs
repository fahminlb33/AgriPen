using AgriPen.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AgriPen.Domain.Entities;

public class User : IEntityModel
{
    [Key, StringLength(26)]
    public Ulid Id { get; set; }

    [MaxLength(100)]
    public string Username { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }
    [MaxLength(60)]
    public string HashedPassword { get; set; }
    public UserRole Role { get; set; }

    public DateTimeOffset? LastLoginAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
