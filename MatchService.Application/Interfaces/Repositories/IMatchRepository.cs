using MatchService.Domain.Entities;

namespace MatchService.Application.Interfaces.Repositories;

public interface IMatchRepository
{
    Task<Match?> GetByMatchNoAsync(int matchNo);

    Task<List<Match>> GetLiveMatchesAsync();

    Task<Match?> GetCurrentLiveMatchAsync();

    Task<List<Match>> GetUpcomingMatchesAsync();

    Task AddAsync(Match match);

    void Update(Match match);
}