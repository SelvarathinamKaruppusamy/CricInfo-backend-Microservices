using MatchService.Domain.Entities;

namespace MatchService.Application.Interfaces.Repositories;

public interface IMatchTeamRepository
{
    Task<MatchTeam?> GetAsync(
        int teamId,
        int matchNo);

    Task<List<MatchTeam>> GetByMatchNoAsync(
        int matchNo);

    Task<List<MatchTeam>> GetAllAsync();

    Task AddAsync(MatchTeam team);

    void Update(MatchTeam team);
}