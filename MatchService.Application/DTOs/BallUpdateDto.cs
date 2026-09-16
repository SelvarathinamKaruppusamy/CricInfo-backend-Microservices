namespace MatchService.Application.DTOs;

public class BallUpdateDto
{
    public int MatchNo { get; set; }

    public string BallResult { get; set; } = string.Empty;
}