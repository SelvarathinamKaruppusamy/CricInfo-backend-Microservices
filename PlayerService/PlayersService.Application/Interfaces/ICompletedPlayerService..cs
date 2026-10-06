using PlayersService.DTOs;

namespace PlayersService.Application.Interfaces;

public interface ICompletedPlayerService
{
    Task<List<CompletedPlayerDto>> GetCompletedPlayersAsync(int matchNo);
}