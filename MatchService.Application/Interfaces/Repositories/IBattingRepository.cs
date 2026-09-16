using MatchService.Domain.Entities;

namespace MatchService.Application.Interfaces.Repositories;

public interface IBattingRepository
{
    Task<List<Batting>> GetByMatchNoAsync(int matchNo);

    Task<List<Batting>> GetByTeamAsync(
        int teamId,
        int matchNo);

    Task AddAsync(Batting batting);

    void Update(Batting batting);
}