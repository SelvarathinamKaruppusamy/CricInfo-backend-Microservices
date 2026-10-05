using MatchService.Application.DTOs;
using MatchService.Application.Interfaces.Services;
using MatchService.Application.Memory;
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
    [HttpPut("change-bowler")]
    public async Task<IActionResult> ChangeBowler(
    [FromBody] ChangeBowlerDto dto)
    {
        try
        {
            var result = await _matchService.ChangeBowlerAsync(dto);

            if (!result)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Unable to change bowler."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Bowler changed successfully."
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("========== CHANGE BOWLER ERROR ==========");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException?.Message);
            Console.WriteLine(ex.InnerException?.InnerException?.Message);
            Console.WriteLine("=========================================");

            return StatusCode(500, new
            {
                success = false,
                message = ex.Message,
                innerException = ex.InnerException?.Message,
                innerInnerException = ex.InnerException?.InnerException?.Message
            });
        }
    }
    [HttpPut("toss")]
    public async Task<IActionResult> UpdateToss(
    [FromBody] TossDto dto)
    {
        try
        {
            var result = await _matchService.UpdateTossAsync(dto);

            if (!result)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Unable to update toss."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Toss updated successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = ex.Message
            });
        }
    }
    [HttpPut("start/{matchNo}")]
    public async Task<IActionResult> StartMatch(int matchNo)
    {
        try
        {
            var result = await _matchService.StartMatchAsync(matchNo);

            if (!result)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Unable to start match."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Match started successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = ex.Message,
                innerException = ex.InnerException?.Message
            });
        }
    }
    [HttpPut("start-second-innings/{matchNo}")]
    public async Task<IActionResult> StartSecondInnings(int matchNo)
    {
        try
        {
            var result =
                await _matchService.StartSecondInningsAsync(matchNo);

            if (!result)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Unable to start second innings."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Second innings started successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = ex.Message,
                innerException = ex.InnerException?.Message
            });
        }
    }
    [HttpGet("balls/{matchNo}")]
    public IActionResult GetStoredBalls(int matchNo)
    {
        var firstInnings =
            MatchBallStore.FirstInningsBalls
                .GetValueOrDefault(matchNo, new List<string>());

        var secondInnings =
            MatchBallStore.SecondInningsBalls
                .GetValueOrDefault(matchNo, new List<string>());

        return Ok(new
        {
            matchNo,
            firstInnings,
            secondInnings
        });
    }
}