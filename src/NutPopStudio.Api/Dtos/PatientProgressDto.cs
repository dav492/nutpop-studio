namespace NutPopStudio.Api.Dtos;

public class PatientProgressDto
{
    public int PatientId { get; set; }

    public decimal StartingWeight { get; set; }

    public decimal CurrentWeight { get; set; }

    public decimal WeightDifference { get; set; }

    public decimal StartingBmi { get; set; }

    public decimal CurrentBmi { get; set; }
}