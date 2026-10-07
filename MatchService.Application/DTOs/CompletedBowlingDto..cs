namespace MatchService.Application.DTOs;

public class CompletedBowlingDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }
    public string Overs { get; set; } = string.Empty;
    public int Balls { get; set; }
    public int Maidens { get; set; }
    public int RunsConceded { get; set; }
    public int Wickets { get; set; }
    public decimal Economy { get; set; }
}