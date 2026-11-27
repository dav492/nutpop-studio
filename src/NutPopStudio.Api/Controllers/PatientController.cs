using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Models;
using System.Linq;
using NutPopStudio.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using NutPopStudio.Api.Services;

namespace NutPopStudio.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly PatientService _patientService;

    public PatientsController(PatientService patientService)
    {
        _patientService = patientService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
    {
        return await _patientService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);

        if (patient == null)
            return NotFound();

        return patient;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
    {
        await _patientService.CreateAsync(patient);

        return CreatedAtAction(
            nameof(GetPatient),
            new { id = patient.Id },
            patient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var deleted = await _patientService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("{id}/measurements")]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurementsForPatient(int id)
    {
        var result = await _patientService.GetMeasurementsAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{id}/progress")]
    public async Task<ActionResult<PatientProgressDto>> GetProgress(int id)
    {
        var result = await _patientService.GetProgressAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{id}/dashboard")]
    public async Task<ActionResult<PatientDashboardDto>> GetDashboard(int id)
    {
        var dashboard =
            await _patientService.GetDashboardAsync(id);

        if (dashboard == null)
            return NotFound();

        return Ok(dashboard);
    }
}