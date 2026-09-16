using MatchService.Domain.Entities;

namespace MatchService.Application.Interfaces.Repositories;

public interface IBowlingRepository
{
    Task<List<Bowling>> GetByMatchNoAsync(int matchNo);

    Task<List<Bowling>> GetByTeamAsync(
        int teamId,
        int matchNo);

    Task AddAsync(Bowling bowling);

    void Update(Bowling bowling);
}