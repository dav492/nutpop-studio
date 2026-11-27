using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Dtos;
using NutPopStudio.Api.Models;
using NutPopStudio.Api.Services;

namespace NutPopStudio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly AppointmentService _appointmentService;

    public AppointmentsController(AppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
    {
        return await _appointmentService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);

        if (appointment == null)
            return NotFound();

        return appointment;
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
    {
        var created =
            await _appointmentService
                .CreateAsync(appointment);

        if (created == null)
            return BadRequest("Patient not found.");

        return CreatedAtAction(
            nameof(GetAppointmentById),
            new { id = created.Id },
            created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
    {
        var updated =
            await _appointmentService
                .UpdateAsync(id, appointment);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var deleted =
            await _appointmentService
                .DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<UpcomingAppointmentDto>>> GetUpcomingAppointments()
    {
        return Ok(
            await _appointmentService
                .GetUpcomingAppointmentsAsync());
    }
}