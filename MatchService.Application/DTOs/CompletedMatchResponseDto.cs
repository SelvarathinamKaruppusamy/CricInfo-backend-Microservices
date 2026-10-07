namespace MatchService.Application.DTOs;

public class CompletedMatchResponseDto
{
    public int MatchNo { get; set; }
    public string? Venue { get; set; }
    public string? City { get; set; }
    public DateOnly? Date { get; set; }
    public string? TossWinner { get; set; }
    public string? TossDecision { get; set; }
    public string? Result { get; set; }
    public string? PlayerOfTheMatch { get; set; }
    public string? Status { get; set; }

    public List<CompletedTeamDto> Teams { get; set; } = new();
}