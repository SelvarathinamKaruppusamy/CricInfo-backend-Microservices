using PlayersService.Entities;
using System.Numerics;

namespace PlayersService.Application.Interfaces.Repositories;

public interface IPlayerRepository
{
    Task<List<Player>> GetAllAsync();

    Task<Player?> GetByIdAsync(
        int playerId,
        int teamId,
        int matchNo);

    Task<List<Player>> GetByMatchNoAsync(
        int matchNo);

    Task<List<Player>> GetByTeamAsync(
        int teamId,
        int matchNo);

    Task<Player?> GetNextBatterAsync(
        int teamId,
        int matchNo);

    Task<Player> CreateAsync(
        Player player);

    Task<bool> UpdateAsync(
        Player player);

    Task<bool> DeleteAsync(
        int playerId,
        int teamId,
        int matchNo);
}