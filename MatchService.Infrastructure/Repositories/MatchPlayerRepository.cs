using MatchService.Application.Interfaces.Repositories;
using MatchService.Domain.Entities;
using MatchService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchService.Infrastructure.Repositories;

public class MatchPlayerRepository : IMatchPlayerRepository
{
    private readonly MatchDbContext _context;

    public MatchPlayerRepository(MatchDbContext context)
    {
        _context = context;
    }

    public async Task<MatchPlayer?> GetAsync(
        int playerId,
        int teamId,
        int matchNo)
    {
        return await _context.MatchPlayers
            .FirstOrDefaultAsync(x =>
                x.PlayerId == playerId &&
                x.TeamId == teamId &&
                x.MatchNo == matchNo);
    }

    public async Task<List<MatchPlayer>> GetByMatchNoAsync(
        int matchNo)
    {
        return await _context.MatchPlayers
            .Where(x => x.MatchNo == matchNo)
            .ToListAsync();
    }

    public async Task<List<MatchPlayer>> GetByTeamAsync(
        int teamId,
        int matchNo)
    {
        return await _context.MatchPlayers
            .Where(x =>
                x.TeamId == teamId &&
                x.MatchNo == matchNo)
            .ToListAsync();
    }

    public async Task<MatchPlayer?> GetNextBatterAsync(
        int teamId,
        int matchNo)
    {
        return await _context.MatchPlayers
            .Where(x =>
                x.TeamId == teamId &&
                x.MatchNo == matchNo &&
                x.Status == "NotOut")
            .OrderBy(x => x.PlayerId)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(MatchPlayer player)
    {
        await _context.MatchPlayers.AddAsync(player);
    }

    public void Update(MatchPlayer player)
    {
        _context.MatchPlayers.Update(player);
    }
}