using System.ComponentModel.DataAnnotations.Schema;

namespace NutPopStudio.Api.Models;

public class Measurement
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public decimal WeightKg { get; set; }
    public decimal HeightCm { get; set; }
    [NotMapped]
    public decimal Bmi =>
        HeightCm > 0
            ? Math.Round(
                WeightKg /
                (HeightCm / 100 * HeightCm / 100),
                2)
            : 0;
    public decimal? FatMassPercentage { get; set; }
    public string? Notes { get; set; }
    public DateTime MeasurementDate { get; set; } = DateTime.UtcNow;
    public Patient? Patient { get; set; }
}