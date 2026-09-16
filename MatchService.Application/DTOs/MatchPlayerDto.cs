namespace MatchService.Application.DTOs;

public class MatchPlayerDto
{
    public int MatchNo { get; set; }

    public int TeamId { get; set; }

    public int PlayerId { get; set; }

    public string? Name { get; set; }

    public string? Role { get; set; }

    public int? Runs { get; set; }

    public int? Balls { get; set; }

    public int? Fours { get; set; }

    public int? Sixes { get; set; }

    public decimal? StrikeRate { get; set; }

    public string? Status { get; set; }

    public decimal? Overs { get; set; }

    public int? BowlingBalls { get; set; }

    public int? Wickets { get; set; }

    public int? Maidens { get; set; }

    public int? RunsConceded { get; set; }

    public decimal? Economy { get; set; }
}