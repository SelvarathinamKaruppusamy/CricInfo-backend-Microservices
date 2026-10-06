using PlayersService.Entities;

namespace PlayersService.Application.Interfaces.Repositories;

public interface IBowlingRepository
{
    Task<List<Bowling>> GetByMatchNoAsync(int matchNo);
}