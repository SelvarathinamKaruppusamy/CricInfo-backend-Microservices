using MatchService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatchService.Infrastructure.Persistence;

public class MatchDbContext : DbContext
{
    public MatchDbContext(DbContextOptions<MatchDbContext> options)
        : base(options)
    {
    }

    public DbSet<Match> Matches { get; set; }

    public DbSet<MatchTeam> MatchTeams { get; set; }

    public DbSet<MatchPlayer> MatchPlayers { get; set; }

    public DbSet<Batting> Battings { get; set; }

    public DbSet<Bowling> Bowlings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MatchDbContext).Assembly);
    }
}