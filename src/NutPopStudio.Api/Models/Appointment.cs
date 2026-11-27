using System.Text.Json.Serialization;

namespace NutPopStudio.Api.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Notes { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    [JsonIgnore]
    public Patient? Patient { get; set; }
}