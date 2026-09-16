namespace MatchService.Application.DTOs;

public class MatchUpdateDto
{
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
}