using MatchService.Application.Interfaces.Repositories;
using MatchService.Domain.Entities;
using MatchService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchService.Infrastructure.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly MatchDbContext _context;

    public MatchRepository(MatchDbContext context)
    {
        _context = context;
    }

    public async Task<Match?> GetByMatchNoAsync(int matchNo)
    {
        return await _context.Matches
            .FirstOrDefaultAsync(x => x.MatchNo == matchNo);
    }

    public async Task<List<Match>> GetLiveMatchesAsync()
    {
        return await _context.Matches
            .Where(x => x.Status == "Live")
            .ToListAsync();
    }

    public async Task<Match?> GetCurrentLiveMatchAsync()
    {
        return await _context.Matches
            .FirstOrDefaultAsync(x => x.Status == "Live");
    }

    public async Task<List<Match>> GetUpcomingMatchesAsync()
    {
        return await _context.Matches
            .Where(x => x.Status == "Upcoming")
            .ToListAsync();
    }

    public async Task AddAsync(Match match)
    {
        await _context.Matches.AddAsync(match);
    }

    public void Update(Match match)
    {
        _context.Matches.Update(match);
    }
}