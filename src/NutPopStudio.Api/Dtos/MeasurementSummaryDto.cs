namespace NutPopStudio.Api.Dtos;

public class MeasurementSummaryDto
{
    public int Id { get; set; }

    public decimal WeightKg { get; set; }

    public decimal HeightCm { get; set; }

    public decimal Bmi { get; set; }

    public decimal? FatMassPercentage { get; set; }

    public DateTime MeasurementDate { get; set; }
}