namespace MatchService.Domain.Entities;

public class MatchTeam
{
    public int MatchNo { get; set; }

    public int TeamId { get; set; }

    public string? FullName { get; set; }

    public string? ShortName { get; set; }

    public string? Logo { get; set; }

    public string? Scores { get; set; }

    public int? Runs { get; set; }

    public int? Wickets { get; set; }

    public int? Extras { get; set; }

    public decimal? Overs { get; set; }

    public int? Balls { get; set; }

    public string? MatchStatus { get; set; }
}