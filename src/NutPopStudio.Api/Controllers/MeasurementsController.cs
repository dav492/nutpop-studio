using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MeasurementsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
    {
        return await _context.Measurements.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Measurement>> GetMeasurement(int id)
    {
        var measurement = await _context.Measurements.FindAsync(id);

        if (measurement == null)
            return NotFound();

        return measurement;
    }

    [HttpPost]
    public async Task<ActionResult<Measurement>> CreateMeasurement(Measurement measurement)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == measurement.PatientId);

        if (!patientExists)
            return BadRequest("Patient not found.");

        _context.Measurements.Add(measurement);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetMeasurement),
            new { id = measurement.Id },
            measurement);
    }
}