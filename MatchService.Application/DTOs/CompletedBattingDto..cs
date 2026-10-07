namespace MatchService.Application.DTOs;

public class CompletedBattingDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }
    public int Runs { get; set; }
    public int Balls { get; set; }
    public int Fours { get; set; }
    public int Sixes { get; set; }
    public decimal StrikeRate { get; set; }
    public string? Status { get; set; }
}