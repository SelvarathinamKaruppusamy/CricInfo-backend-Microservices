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

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine("========== EF SAVE ERROR ==========");

            foreach (var entry in _context.ChangeTracker.Entries())
            {
                Console.WriteLine(
                    $"Entity: {entry.Entity.GetType().Name} | State: {entry.State}");

                foreach (var property in entry.Properties)
                {
                    if (property.IsModified)
                    {
                        Console.WriteLine(
                            $"  Modified: {property.Metadata.Name} = {property.CurrentValue}");
                    }
                }
            }

            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine($"INNER: {ex.InnerException?.Message}");

            throw;
        }
    }
}