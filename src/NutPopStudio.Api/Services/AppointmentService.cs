using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Dtos;
using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Services;

public class AppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Appointment>> GetAllAsync()
    {
        return await _context.Appointments
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Appointment?> CreateAsync(
        Appointment appointment)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == appointment.PatientId);

        if (!patientExists)
            return null;

        _context.Appointments.Add(appointment);

        await _context.SaveChangesAsync();

        return appointment;
    }

    public async Task<bool> UpdateAsync(
        int id,
        Appointment appointment)
    {
        var existing = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id);

        if (existing == null)
            return false;

        existing.AppointmentDate =
            appointment.AppointmentDate;

        existing.Status =
            appointment.Status;

        existing.Notes =
            appointment.Notes;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null)
            return false;

        _context.Appointments.Remove(appointment);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<UpcomingAppointmentDto>>
        GetUpcomingAppointmentsAsync()
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.AppointmentDate >= DateTime.Now)
            .OrderBy(a => a.AppointmentDate)
            .Select(a => new UpcomingAppointmentDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName =
                    $"{a.Patient!.Name} {a.Patient.Surname}",
                AppointmentDate = a.AppointmentDate,
                Notes = a.Notes
            })
            .ToListAsync();
    }
}