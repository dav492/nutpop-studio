using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Endpoints;

public static class AppointmentsEndpoints
{
    public static IEndpointRouteBuilder
        MapAppointmentsEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/appointments",
            async (AppDbContext db) =>
            {
                return await db.Appointments
                    .OrderBy(a => a.AppointmentDate)
                    .ToListAsync();
            });

        endpoints.MapGet(
            "/patients/{patientId:int}/appointments",
            async (
                int patientId,
                AppDbContext db) =>
            {
                return await db.Appointments
                    .Where(a => a.PatientId == patientId)
                    .OrderBy(a => a.AppointmentDate)
                    .ToListAsync();
            });

        endpoints.MapPost("/appointments",
            async (
                Appointment appointment,
                AppDbContext db) =>
            {
                var patientExists =
                    await db.Patients.AnyAsync(
                        p => p.Id == appointment.PatientId);

                if (!patientExists)
                {
                    return Results.BadRequest(
                        "Patient not found.");
                }

                db.Appointments.Add(appointment);

                await db.SaveChangesAsync();

                return Results.Created(
                    $"/appointments/{appointment.Id}",
                    appointment);
            });

        return endpoints;
    }
}