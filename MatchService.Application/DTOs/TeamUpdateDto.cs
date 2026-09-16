namespace MatchService.Application.DTOs;

public class TeamUpdateDto
{
    public string? Scores { get; set; }

    public int? Runs { get; set; }

    public int? Wickets { get; set; }

    public int? Extras { get; set; }

    public decimal? Overs { get; set; }

    public int? Balls { get; set; }

    public int? WinCount { get; set; }

    public int? LossCount { get; set; }

    public int? TotalMatch { get; set; }

    public string? MatchStatus { get; set; }
}