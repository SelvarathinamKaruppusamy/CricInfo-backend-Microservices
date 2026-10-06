using PlayersService.Application.Interfaces.Repositories;
using PlayersService.Application.Services;
using PlayersService.DTOs;
using PlayersService.Entities;
using System.Numerics;

namespace PlayersService.Application.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _repository;

    public PlayerService(
        IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Player>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Player?> GetByIdAsync(
        int playerId,
        int teamId,
        int matchNo)
    {
        return await _repository.GetByIdAsync(
            playerId,
            teamId,
            matchNo);
    }

    public async Task<List<Player>> GetByMatchNoAsync(
        int matchNo)
    {
        return await _repository.GetByMatchNoAsync(
            matchNo);
    }

    public async Task<List<Player>> GetByTeamAsync(
        int teamId,
        int matchNo)
    {
        return await _repository.GetByTeamAsync(
            teamId,
            matchNo);
    }

    public async Task<Player?> GetNextBatterAsync(
        int teamId,
        int matchNo)
    {
        return await _repository.GetNextBatterAsync(
            teamId,
            matchNo);
    }

    public async Task<bool> UpdateAsync(
        int playerId,
        int teamId,
        int matchNo,
        UpdatePlayerDto dto)
    {
        var player = await _repository.GetByIdAsync(
            playerId,
            teamId,
            matchNo);

        if (player == null)
            return false;

        player.runs = dto.runs;
        player.balls = dto.balls;
        player.fours = dto.fours;
        player.sixes = dto.sixes;
        player.strikeRate = dto.strikeRate;
        player.status = dto.status;
        player.overs = dto.overs;
        player.wickets = dto.wickets;
        player.maidens = dto.maidens;
        player.runsConceded = dto.runsConceded;
        player.economy = dto.economy;
        player.bowlingBalls = dto.bowlingBalls;

        return await _repository.UpdateAsync(player);
    }

    public async Task<bool> DeleteAsync(
        int playerId,
        int teamId,
        int matchNo)
    {
        return await _repository.DeleteAsync(
            playerId,
            teamId,
            matchNo);
    }
}

