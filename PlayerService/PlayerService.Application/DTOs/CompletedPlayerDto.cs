namespace PlayersService.DTOs;

public class CompletedPlayerDto
{
    public int playerId { get; set; }

    public int TeamId { get; set; }

    public int matchNo { get; set; }

    public string? name { get; set; }

    public string? role { get; set; }

    public BattingDto? Batting { get; set; }

    public BowlingDto? Bowling { get; set; }
}