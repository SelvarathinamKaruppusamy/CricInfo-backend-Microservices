namespace MatchService.Application.DTOs;

public class MatchDto
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

    public int? CurrentInnings { get; set; }

    public int? CurrentBattingTeamIndex { get; set; }

    public int? CurrentBowlingTeamIndex { get; set; }

    public int? StrikerPlayerId { get; set; }

    public int? NonStrikerPlayerId { get; set; }

    public int? CurrentBowlerPlayerId { get; set; }

    public List<MatchTeamDto> Teams { get; set; } = new();
}