using MatchService.Application.DTOs;
using MatchService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchService.API.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpGet("live")]
    public async Task<IActionResult> GetLiveMatch()
    {
        var result = await _matchService.GetLiveMatchAsync();

        if (result == null)
            return NotFound("Live match not found.");

        return Ok(result);
    }

    [HttpPost("ball")]
    public async Task<IActionResult> ProcessBall(
        [FromBody] BallUpdateDto dto)
    {
        var result = await _matchService.ProcessBallAsync(dto);

        if (!result)
            return BadRequest("Unable to process ball.");

        return Ok(new
        {
            success = true,
            message = "Ball processed successfully."
        });
    }
}