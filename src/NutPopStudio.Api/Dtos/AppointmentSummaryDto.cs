using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Dtos;

public class AppointmentSummaryDto
{
    public int Id { get; set; }

    public DateTime AppointmentDate { get; set; }

    public AppointmentStatus Status { get; set; }

    public string? Notes { get; set; }
}