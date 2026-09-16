using MatchService.Application.Interfaces.Repositories;
using MatchService.Domain.Entities;
using MatchService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchService.Infrastructure.Repositories;

public class BattingRepository : IBattingRepository
{
    private readonly MatchDbContext _context;

    public BattingRepository(MatchDbContext context)
    {
        _context = context;
    }

    public async Task<List<Batting>> GetByMatchNoAsync(
        int matchNo)
    {
        return await _context.Battings
            .Where(x => x.MatchNo == matchNo)
            .ToListAsync();
    }

    public async Task<List<Batting>> GetByTeamAsync(
        int teamId,
        int matchNo)
    {
        return await _context.Battings
            .Where(x =>
                x.TeamId == teamId &&
                x.MatchNo == matchNo)
            .ToListAsync();
    }

    public async Task AddAsync(Batting batting)
    {
        await _context.Battings.AddAsync(batting);
    }

    public void Update(Batting batting)
    {
        _context.Battings.Update(batting);
    }
}