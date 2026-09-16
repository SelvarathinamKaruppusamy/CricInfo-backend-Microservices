using MatchService.Domain.Entities;

namespace MatchService.Application.Interfaces.Repositories;

public interface IMatchPlayerRepository
{
    Task<MatchPlayer?> GetAsync(
        int playerId,
        int teamId,
        int matchNo);

    Task<List<MatchPlayer>> GetByMatchNoAsync(
        int matchNo);

    Task<List<MatchPlayer>> GetByTeamAsync(
        int teamId,
        int matchNo);

    Task<MatchPlayer?> GetNextBatterAsync(
        int teamId,
        int matchNo);

    Task AddAsync(MatchPlayer player);

    void Update(MatchPlayer player);
}