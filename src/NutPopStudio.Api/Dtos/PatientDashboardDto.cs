using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Dtos;

public class PatientDashboardDto
{
    public PatientSummaryDto? Patient { get; set; }

    public MeasurementSummaryDto? LatestMeasurement { get; set; }

    public PatientProgressDto? Progress { get; set; }

    public AppointmentSummaryDto? NextAppointment { get; set; }
}
