using PlayersService.Entities;

namespace PlayersService.Application.Interfaces.Repositories;

public interface IBattingRepository
{
    Task<List<Batting>> GetByMatchNoAsync(int matchNo);
}