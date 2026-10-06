namespace PlayersService.DTOs;

public class BowlingDto
{
    public int playerId { get; set; }

    public int TeamId { get; set; }

    public int matchNo { get; set; }

    public string? name { get; set; }

    public string? role { get; set; }

    public string? overs { get; set; }

    public int? balls { get; set; }

    public int? maidens { get; set; }

    public int? runsConceded { get; set; }

    public int? wickets { get; set; }

    public decimal? economy { get; set; }
}