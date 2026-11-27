using System.ComponentModel.DataAnnotations;

namespace NutPopStudio.Api.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Admin;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
}