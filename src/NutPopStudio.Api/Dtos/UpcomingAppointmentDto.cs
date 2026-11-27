namespace NutPopStudio.Api.Dtos;

public class UpcomingAppointmentDto
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }

    public string? Notes { get; set; }
}