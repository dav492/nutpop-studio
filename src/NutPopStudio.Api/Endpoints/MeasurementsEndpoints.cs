using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Endpoints;

public static class MeasurementsEndpoints
{
    public static IEndpointRouteBuilder
        MapMeasurementsEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/measurements",
            async (
                Measurement measurement,
                AppDbContext db) =>
            {
                var patientExists =
                    await db.Patients.AnyAsync(
                        p => p.Id == measurement.PatientId);

                if (!patientExists)
                {
                    return Results.BadRequest(
                        "Patient not found.");
                }

                db.Measurements.Add(measurement);

                await db.SaveChangesAsync();

                return Results.Created(
                    $"/measurements/{measurement.Id}",
                    measurement);
            });

        endpoints.MapGet(
            "/patients/{patientId:int}/measurements",
            async (
                int patientId,
                AppDbContext db) =>
            {
                var measurements =
                    await db.Measurements
                        .Where(x => x.PatientId == patientId)
                        .OrderByDescending(
                            x => x.MeasurementDate)
                        .ToListAsync();

                return Results.Ok(measurements);
            });

        return endpoints;
    }
}