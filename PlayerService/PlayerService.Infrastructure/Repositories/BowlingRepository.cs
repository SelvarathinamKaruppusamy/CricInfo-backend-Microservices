using Microsoft.EntityFrameworkCore;
using PlayersService.Application.Interfaces.Repositories;
using PlayersService.Entities;
using PlayersService.Infrastructure.Persistence;

namespace PlayersService.Infrastructure.Repositories;

public class BowlingRepository : IBowlingRepository
{
    private readonly PlayersDbContext _context;

    public BowlingRepository(PlayersDbContext context)
    {
        _context = context;
    }

    public async Task<List<Bowling>> GetByMatchNoAsync(
        int matchNo)
    {
        return await _context.Bowling
            .Where(x => x.matchNo == matchNo)
            .OrderBy(x => x.TeamId)
            .ThenBy(x => x.playerId)
            .ToListAsync();
    }
}