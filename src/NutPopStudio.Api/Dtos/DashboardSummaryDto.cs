namespace NutPopStudio.Api.Dtos;

public class DashboardSummaryDto
{
    public int TotalPatients { get; set; }

    public int AppointmentsToday { get; set; }

    public int AppointmentsThisWeek { get; set; }

    public int NewPatientsThisMonth { get; set; }

    public List<UpcomingAppointmentDto> UpcomingAppointments { get; set; }
    = [];
}