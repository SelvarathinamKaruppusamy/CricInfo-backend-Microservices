using Microsoft.EntityFrameworkCore;
using PlayersService.Entities;
namespace PlayersService.Infrastructure.Persistence;

public class PlayersDbContext : DbContext
{
    public PlayersDbContext(
        DbContextOptions<PlayersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Batting> Batting { get; set; }
    public DbSet<Bowling> Bowling { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Players");

            entity.HasKey(x => new
            {
                x.TeamId,
                x.matchNo,
                x.playerId
            });

            entity.Property(x => x.id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.playerId)
                .ValueGeneratedNever();

            entity.Property(x => x.TeamId)
                .ValueGeneratedNever();

            entity.Property(x => x.matchNo)
                .ValueGeneratedNever();

            entity.Property(x => x.name)
                .HasMaxLength(255);

            entity.Property(x => x.role)
                .HasMaxLength(255);

            entity.Property(x => x.status)
                .HasMaxLength(255);
        });
        modelBuilder.Entity<Batting>(entity =>
        {
            entity.ToTable("Batting");

            entity.HasKey(x => x.id);

            entity.Property(x => x.id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.playerId)
                .ValueGeneratedNever();

            entity.Property(x => x.TeamId)
                .ValueGeneratedNever();

            entity.Property(x => x.matchNo)
                .ValueGeneratedNever();

            entity.Property(x => x.name)
                .HasMaxLength(255);

            entity.Property(x => x.role)
                .HasMaxLength(255);

            entity.Property(x => x.status)
                .HasMaxLength(255);
        });
        modelBuilder.Entity<Bowling>(entity =>
        {
            entity.ToTable("Bowling");

            entity.HasKey(x => x.id);

            entity.Property(x => x.id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.playerId)
                .ValueGeneratedNever();

            entity.Property(x => x.TeamId)
                .ValueGeneratedNever();

            entity.Property(x => x.matchNo)
                .ValueGeneratedNever();

            entity.Property(x => x.name)
                .HasMaxLength(255);

            entity.Property(x => x.role)
                .HasMaxLength(255);

            entity.Property(x => x.overs)
                .HasMaxLength(255);
        });
    }
}