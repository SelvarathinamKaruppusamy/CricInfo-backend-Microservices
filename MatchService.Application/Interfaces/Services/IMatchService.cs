using MatchService.Application.DTOs;

namespace MatchService.Application.Interfaces.Services;

public interface IMatchService
{
    Task<MatchDto?> GetLiveMatchAsync();

    Task<bool> ProcessBallAsync(BallUpdateDto dto);

    Task<bool> UpdateMatchAsync(int matchNo, MatchUpdateDto dto);

    Task<bool> StartMatchAsync(int matchNo);

    Task<bool> UpdateTossAsync(TossDto dto);

    Task<bool> ChangeBowlerAsync(ChangeBowlerDto dto);

    Task<bool> StartSecondInningsAsync(int matchNo);

    //Task<bool> CompleteMatchAsync(CompletedMatchDto dto);

    Task<bool> UpdatePlayerOfTheMatchAsync(
        int matchNo,
        PlayerOfTheMatchDto dto);

    Task<bool> PromoteUpcomingMatchAsync(int matchNo);

    Task<List<MatchDto>> GetUpcomingMatchesAsync();
}