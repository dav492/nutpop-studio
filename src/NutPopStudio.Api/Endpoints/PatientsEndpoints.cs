using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Endpoints;

public static class PatientsEndpoints
{
    public static IEndpointRouteBuilder MapPatientsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/patients",
            async (AppDbContext db) =>
                await db.Patients.ToListAsync());

        endpoints.MapGet("/patients/{id:int}",
            async (int id, AppDbContext db) =>
            {
                var patient = await db.Patients
                    .FirstOrDefaultAsync(x => x.Id == id);

                return patient is null
                    ? Results.NotFound()
                    : Results.Ok(patient);
            });

        endpoints.MapPost("/patients",
            async (Patient patient, AppDbContext db) =>
            {
                db.Patients.Add(patient);

                await db.SaveChangesAsync();

                return Results.Created(
                    $"/patients/{patient.Id}",
                    patient);
            });

        endpoints.MapPut("/patients/{id:int}",
            async (
                int id,
                Patient request,
                AppDbContext db) =>
            {
                var patient = await db.Patients
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (patient is null)
                    return Results.NotFound();

                patient.Name = request.Name;
                patient.Surname = request.Surname;
                patient.Email = request.Email;
                patient.Phone = request.Phone;
                patient.DateOfBirth = request.DateOfBirth;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

        endpoints.MapDelete("/patients/{id:int}",
            async (int id, AppDbContext db) =>
            {
                var patient = await db.Patients
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (patient is null)
                    return Results.NotFound();

                db.Patients.Remove(patient);

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

        return endpoints;
    }
}