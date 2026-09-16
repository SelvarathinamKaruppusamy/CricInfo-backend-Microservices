using MatchService.Application.Interfaces;
using MatchService.Infrastructure.Persistence;

namespace MatchService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MatchDbContext _context;

    public UnitOfWork(MatchDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}