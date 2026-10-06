using PlayersService.DTOs;
using PlayersService.Entities;
using System.Numerics;

namespace PlayersService.Application.Services;

public interface IPlayerService
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

    Task<bool> UpdateAsync(
        int playerId,
        int teamId,
        int matchNo,
        UpdatePlayerDto dto);

    Task<bool> DeleteAsync(
        int playerId,
        int teamId,
        int matchNo);
}