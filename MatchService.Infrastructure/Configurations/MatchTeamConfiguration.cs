using MatchService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchService.Infrastructure.Configurations;

public class MatchTeamConfiguration : IEntityTypeConfiguration<MatchTeam>
{
    public void Configure(EntityTypeBuilder<MatchTeam> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(x => new
        {
            x.TeamId,
            x.MatchNo
        });

        builder.Property(x => x.TeamId)
            .HasColumnName("TeamId")
            .ValueGeneratedNever();

        builder.Property(x => x.MatchNo)
            .HasColumnName("matchNo")
            .ValueGeneratedNever();

        // The existing Teams table has an identity column named "id".
        // MatchTeam does not map that column.
        builder.Ignore("id");

        builder.Property(x => x.FullName)
            .HasColumnName("fullName")
            .HasMaxLength(100);

        builder.Property(x => x.ShortName)
            .HasColumnName("shortName")
            .HasMaxLength(50);

        builder.Property(x => x.Logo)
            .HasColumnName("logo")
            .HasMaxLength(500);

        builder.Property(x => x.Scores)
            .HasColumnName("scores")
            .HasMaxLength(50);

        builder.Property(x => x.Runs)
            .HasColumnName("runs");

        builder.Property(x => x.Wickets)
            .HasColumnName("wickets");

        builder.Property(x => x.Extras)
            .HasColumnName("extras");

        builder.Property(x => x.Overs)
            .HasColumnName("overs")
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Balls)
            .HasColumnName("balls");

        builder.Property(x => x.MatchStatus)
            .HasColumnName("matchStatus")
            .HasMaxLength(50);
    }
}