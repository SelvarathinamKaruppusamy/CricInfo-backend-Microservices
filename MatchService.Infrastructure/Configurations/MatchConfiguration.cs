using MatchService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchService.Infrastructure.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(x => x.MatchNo);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.MatchNo)
            .ValueGeneratedNever();

        builder.Property(x => x.Venue)
            .HasMaxLength(255);

        builder.Property(x => x.City)
            .HasMaxLength(100);

        builder.Property(x => x.TossWinner)
            .HasMaxLength(100);

        builder.Property(x => x.TossDecision)
            .HasMaxLength(50);

        builder.Property(x => x.Result)
            .HasMaxLength(255);

        builder.Property(x => x.PlayerOfTheMatch)
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .HasMaxLength(50);
    }
}