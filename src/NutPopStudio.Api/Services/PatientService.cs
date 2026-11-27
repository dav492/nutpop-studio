using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Dtos;
using NutPopStudio.Api.Models;

namespace NutPopStudio.Api.Services;

public class PatientService
{
    private readonly AppDbContext _context;

    public PatientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Patient>> GetAllAsync()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<Patient> CreateAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return patient;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);

        if (patient == null)
            return false;

        _context.Patients.Remove(patient);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<MeasurementSummaryDto>?> GetMeasurementsAsync(int patientId)
    {
        var patientExists = await _context.Patients.AnyAsync(p => p.Id == patientId);

        if (!patientExists)
            return null;

        return await _context.Measurements
            .Where(m => m.PatientId == patientId)
            .OrderByDescending(m => m.MeasurementDate)
            .Select(m => new MeasurementSummaryDto
            {
                Id = m.Id,
                WeightKg = m.WeightKg,
                HeightCm = m.HeightCm,
                Bmi = m.Bmi,
                FatMassPercentage = m.FatMassPercentage,
                MeasurementDate = m.MeasurementDate
            })
            .ToListAsync();
    }

    public async Task<PatientProgressDto?> GetProgressAsync(int patientId)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == patientId);

        if (!patientExists)
            return null;

        var measurements = await _context.Measurements
            .Where(m => m.PatientId == patientId)
            .OrderBy(m => m.MeasurementDate)
            .ToListAsync();

        if (!measurements.Any())
        {
            return new PatientProgressDto
            {
                PatientId = patientId,
                StartingWeight = 0,
                CurrentWeight = 0,
                WeightDifference = 0,
                StartingBmi = 0,
                CurrentBmi = 0
            };
        }

        var first = measurements.First();
        var last = measurements.Last();

        return new PatientProgressDto
        {
            PatientId = patientId,
            StartingWeight = first.WeightKg,
            CurrentWeight = last.WeightKg,
            WeightDifference = last.WeightKg - first.WeightKg,
            StartingBmi = first.Bmi,
            CurrentBmi = last.Bmi
        };
    }

    public async Task<PatientDashboardDto?> GetDashboardAsync(int patientId)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
            return null;

        var latestMeasurement = await _context.Measurements
            .Where(m => m.PatientId == patientId)
            .OrderByDescending(m => m.MeasurementDate)
            .FirstOrDefaultAsync();

        var progress = await GetProgressAsync(patientId);

        var nextAppointment = await _context.Appointments
            .Where(a =>
                a.PatientId == patientId &&
                a.AppointmentDate >= DateTime.Now)
            .OrderBy(a => a.AppointmentDate)
            .FirstOrDefaultAsync();

        return new PatientDashboardDto
        {
            Patient = new PatientSummaryDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Surname = patient.Surname
            },

            LatestMeasurement = latestMeasurement == null
                ? null
                : new MeasurementSummaryDto
                {
                    Id = latestMeasurement.Id,
                    WeightKg = latestMeasurement.WeightKg,
                    HeightCm = latestMeasurement.HeightCm,
                    Bmi = latestMeasurement.Bmi,
                    FatMassPercentage =
                        latestMeasurement.FatMassPercentage,
                    MeasurementDate =
                        latestMeasurement.MeasurementDate
                },

            Progress = progress,

            NextAppointment = nextAppointment == null
                ? null
                : new AppointmentSummaryDto
                {
                    Id = nextAppointment.Id,
                    AppointmentDate =
                        nextAppointment.AppointmentDate,
                    Status = nextAppointment.Status,
                    Notes = nextAppointment.Notes
                }
        };
    }
}