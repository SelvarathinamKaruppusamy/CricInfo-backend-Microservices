using MatchService.Application.Interfaces.Repositories;
using MatchService.Domain.Entities;
using MatchService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchService.Infrastructure.Repositories;

public class MatchTeamRepository : IMatchTeamRepository
{
    private readonly MatchDbContext _context;

    public MatchTeamRepository(MatchDbContext context)
    {
        _context = context;
    }

    public async Task<MatchTeam?> GetAsync(
        int teamId,
        int matchNo)
    {
        return await _context.MatchTeams
            .FirstOrDefaultAsync(x =>
                x.TeamId == teamId &&
                x.MatchNo == matchNo);
    }

    public async Task<List<MatchTeam>> GetByMatchNoAsync(int matchNo) =>
    await _context.MatchTeams
        .Where(x => x.MatchNo == matchNo)
        .OrderBy(x => x.TeamId)
        .ToListAsync();

    public async Task<List<MatchTeam>> GetAllAsync()
    {
        return await _context.MatchTeams
            .ToListAsync();
    }

    public async Task AddAsync(MatchTeam team)
    {
        await _context.MatchTeams.AddAsync(team);
    }

    public void Update(MatchTeam team)
    {
        _context.MatchTeams.Update(team);
    }
}