using TeamService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TeamService.Infrastructure.Persistence
{
    public class TeamDbContext : DbContext
    {
        public TeamDbContext(DbContextOptions<TeamDbContext> options)
            : base(options)
        {
        }

        public DbSet<Teams> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teams>(entity =>
            {
                entity.ToTable("Teams","dbo");
                entity .HasKey(t => t.Id);

            });
        }
    }
}