using Microsoft.EntityFrameworkCore;
using PlayersService.Application.Interfaces.Repositories;
using PlayersService.Entities;
using PlayersService.Infrastructure.Persistence;
namespace PlayersService.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly PlayersDbContext _context;

    public PlayerRepository(
        PlayersDbContext context)
    {
        _context = context;
    }

    public async Task<List<Player>> GetAllAsync()
    {
        return await _context.Players
            .OrderBy(x => x.matchNo)
            .ThenBy(x => x.TeamId)
            .ThenBy(x => x.playerId)
            .ToListAsync();
    }

    public async Task<Player?> GetByIdAsync(
        int playerId,
        int teamId,
        int matchNo)
    {
        return await _context.Players
            .FirstOrDefaultAsync(x =>
                x.playerId == playerId &&
                x.TeamId == teamId &&
                x.matchNo == matchNo);
    }

    public async Task<List<Player>> GetByMatchNoAsync(
        int matchNo)
    {
        return await _context.Players
            .Where(x => x.matchNo == matchNo)
            .OrderBy(x => x.TeamId)
            .ThenBy(x => x.playerId)
            .ToListAsync();
    }

    public async Task<List<Player>> GetByTeamAsync(
        int teamId,
        int matchNo)
    {
        return await _context.Players
            .Where(x =>
                x.TeamId == teamId &&
                x.matchNo == matchNo)
            .OrderBy(x => x.playerId)
            .ToListAsync();
    }

    public async Task<Player?> GetNextBatterAsync(
        int teamId,
        int matchNo)
    {
        return await _context.Players
            .Where(x =>
                x.TeamId == teamId &&
                x.matchNo == matchNo &&
                x.status == "Yet To Play")
            .OrderBy(x => x.playerId)
            .FirstOrDefaultAsync();
    }

    public async Task<Player> CreateAsync(
        Player player)
    {
        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();

        return player;
    }

    public async Task<bool> UpdateAsync(
        Player player)
    {

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(
        int playerId,
        int teamId,
        int matchNo)
    {
        var player = await GetByIdAsync(
            playerId,
            teamId,
            matchNo);

        if (player == null)
            return false;

        _context.Players.Remove(player);

        await _context.SaveChangesAsync();

        return true;
    }
}