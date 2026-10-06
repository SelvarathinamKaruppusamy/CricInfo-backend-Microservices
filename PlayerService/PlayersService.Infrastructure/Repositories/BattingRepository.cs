using Microsoft.EntityFrameworkCore;
using PlayersService.Application.Interfaces.Repositories;
using PlayersService.Entities;
using PlayersService.Infrastructure.Persistence;

namespace PlayersService.Infrastructure.Repositories;

public class BattingRepository : IBattingRepository
{
    private readonly PlayersDbContext _context;

    public BattingRepository(PlayersDbContext context)
    {
        _context = context;
    }

    public async Task<List<Batting>> GetByMatchNoAsync(
    int matchNo)
    {
        return await _context.Batting
            .Where(x => x.matchNo == matchNo)
            .OrderBy(x => x.TeamId)
            .ThenBy(x => x.playerId)
            .ToListAsync();
    }
}