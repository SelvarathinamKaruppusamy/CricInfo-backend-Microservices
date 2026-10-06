using Microsoft.AspNetCore.Mvc;
using PlayersService.Application.Interfaces;
using PlayersService.Application.Services;
using PlayersService.DTOs;

namespace PlayersService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _service;
    private readonly ICompletedPlayerService _completedPlayerService;

    public PlayersController(
    IPlayerService service,
    ICompletedPlayerService completedPlayerService)
    {
        _service = service;
        _completedPlayerService = completedPlayerService;
    }

    // GET: api/Players
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var players = await _service.GetAllAsync();

        return Ok(players);
    }

    // GET:
    // api/Players/1/2/1001
    [HttpGet("{playerId}/team/{teamId}/match/{matchNo}")]
    public async Task<IActionResult> GetById(
        int playerId,
        int teamId,
        int matchNo)
    {
        var player = await _service.GetByIdAsync(
            playerId,
            teamId,
            matchNo);

        if (player == null)
            return NotFound(new
            {
                message = "Player not found"
            });

        return Ok(player);
    }

    // GET:
    // api/Players/match/1001
    [HttpGet("match/{matchNo}")]
    public async Task<IActionResult> GetByMatch(
        int matchNo)
    {
        var players = await _service.GetByMatchNoAsync(
            matchNo);

        return Ok(players);
    }

    // GET:
    // api/Players/team/1/match/1001
    [HttpGet("team/{teamId}/match/{matchNo}")]
    public async Task<IActionResult> GetByTeam(
        int teamId,
        int matchNo)
    {
        var players = await _service.GetByTeamAsync(
            teamId,
            matchNo);

        return Ok(players);
    }

    // GET:
    // api/Players/next-batter/team/1/match/1001
    [HttpGet("next-batter/team/{teamId}/match/{matchNo}")]
    public async Task<IActionResult> GetNextBatter(
        int teamId,
        int matchNo)
    {
        var player = await _service.GetNextBatterAsync(
            teamId,
            matchNo);

        if (player == null)
            return NotFound(new
            {
                message = "No next batter available"
            });

        return Ok(player);
    }

    // PUT:
    // api/Players/1/team/2/match/1001
    [HttpPut("{playerId}/team/{teamId}/match/{matchNo}")]
    public async Task<IActionResult> Update(
        int playerId,
        int teamId,
        int matchNo,
        [FromBody] UpdatePlayerDto dto)
    {
        var updated = await _service.UpdateAsync(
            playerId,
            teamId,
            matchNo,
            dto);

        if (!updated)
            return NotFound(new
            {
                message = "Player not found"
            });

        return Ok(new
        {
            success = true,
            message = "Player updated successfully"
        });
    }

    // DELETE:
    // api/Players/1/team/2/match/1001
    [HttpDelete("{playerId}/team/{teamId}/match/{matchNo}")]
    public async Task<IActionResult> Delete(
        int playerId,
        int teamId,
        int matchNo)
    {
        var deleted = await _service.DeleteAsync(
            playerId,
            teamId,
            matchNo);

        if (!deleted)
            return NotFound(new
            {
                message = "Player not found"
            });

        return Ok(new
        {
            success = true,
            message = "Player deleted successfully"
        });

    }
    // GET:
    // api/Players/completed/25
    [HttpGet("/api/completed/{matchNo}")]
    public async Task<IActionResult> GetCompletedPlayers(int matchNo)
    {
        var players =
            await _completedPlayerService.GetCompletedPlayersAsync(matchNo);

        return Ok(players);
    }
}