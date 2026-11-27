using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Dtos;

namespace NutPopStudio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var today = DateTime.Today;

        var weekEnd = today.AddDays(7);

        var summary = new DashboardSummaryDto
        {
            TotalPatients = await _context.Patients.CountAsync(),

            AppointmentsToday = await _context.Appointments
                .CountAsync(a =>
                    a.AppointmentDate.Date == today),

            AppointmentsThisWeek = await _context.Appointments
                .CountAsync(a =>
                    a.AppointmentDate >= today &&
                    a.AppointmentDate < weekEnd),

            NewPatientsThisMonth = await _context.Patients
                .CountAsync(p =>
                    p.CreatedAt.Year == today.Year &&
                    p.CreatedAt.Month == today.Month),

            UpcomingAppointments = await _context.Appointments
    .Include(a => a.Patient)
    .Where(a => a.AppointmentDate >= DateTime.Now)
    .OrderBy(a => a.AppointmentDate)
    .Take(5)
    .Select(a => new UpcomingAppointmentDto
    {
        Id = a.Id,
        PatientId = a.PatientId,
        PatientName = $"{a.Patient!.Name} {a.Patient.Surname}",
        AppointmentDate = a.AppointmentDate,
        Notes = a.Notes
    })
    .ToListAsync()
        };

        return summary;
    }

}