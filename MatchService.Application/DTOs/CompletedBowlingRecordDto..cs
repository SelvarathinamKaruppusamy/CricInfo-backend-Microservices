namespace MatchService.Application.DTOs;

public class CompletedBowlingRecordDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int TeamId { get; set; }
    public int MatchNo { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    // Completed bowling stores Overs as nvarchar
    public string Overs { get; set; } = string.Empty;

    public int Balls { get; set; }
    public int Maidens { get; set; }
    public int RunsConceded { get; set; }
    public int Wickets { get; set; }
    public decimal Economy { get; set; }
}