using System.ComponentModel.DataAnnotations;

namespace NutPopStudio.Api.Models;

public class Patient
{
    public int Id { get; set; }   
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Surname { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}