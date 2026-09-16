using MatchService.Application.Interfaces.Repositories;
using MatchService.Domain.Entities;
using MatchService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MatchService.Infrastructure.Repositories;

public class BowlingRepository : IBowlingRepository
{
    private readonly MatchDbContext _context;

    public BowlingRepository(MatchDbContext context)
    {
        _context = context;
    }

    public async Task<List<Bowling>> GetByMatchNoAsync(
        int matchNo)
    {
        return await _context.Bowlings
            .Where(x => x.MatchNo == matchNo)
            .ToListAsync();
    }

    public async Task<List<Bowling>> GetByTeamAsync(
        int teamId,
        int matchNo)
    {
        return await _context.Bowlings
            .Where(x =>
                x.TeamId == teamId &&
                x.MatchNo == matchNo)
            .ToListAsync();
    }

    public async Task AddAsync(Bowling bowling)
    {
        await _context.Bowlings.AddAsync(bowling);
    }

    public void Update(Bowling bowling)
    {
        _context.Bowlings.Update(bowling);
    }
}