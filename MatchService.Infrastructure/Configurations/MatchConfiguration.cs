using MatchService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchService.Infrastructure.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        // Primary key
        builder.HasKey(x => x.MatchNo);

        // Existing identity column
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // MatchNo is NOT identity
        builder.Property(x => x.MatchNo)
            .HasColumnName("matchNo")
            .ValueGeneratedNever();

        builder.Property(x => x.Venue)
            .HasColumnName("venue")
            .HasMaxLength(200);

        builder.Property(x => x.City)
            .HasColumnName("city")
            .HasMaxLength(100);

        builder.Property(x => x.Date)
            .HasColumnName("date");

        builder.Property(x => x.TossWinner)
            .HasColumnName("tossWinner")
            .HasMaxLength(100);

        builder.Property(x => x.TossDecision)
            .HasColumnName("tossDecision")
            .HasMaxLength(50);

        builder.Property(x => x.Result)
            .HasColumnName("result")
            .HasMaxLength(200);

        builder.Property(x => x.PlayerOfTheMatch)
            .HasColumnName("playerOfTheMatch")
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(50);

        builder.Property(x => x.CurrentInnings)
            .HasColumnName("currentInnings");

        builder.Property(x => x.CurrentBattingTeamIndex)
            .HasColumnName("currentBattingTeamIndex");

        builder.Property(x => x.CurrentBowlingTeamIndex)
            .HasColumnName("currentBowlingTeamIndex");

        builder.Property(x => x.StrikerPlayerId)
            .HasColumnName("strikerPlayerId");

        builder.Property(x => x.NonStrikerPlayerId)
            .HasColumnName("nonStrikerPlayerId");

        builder.Property(x => x.CurrentBowlerPlayerId)
            .HasColumnName("currentBowlerPlayerId");
    }
}