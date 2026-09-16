using MatchService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchService.Infrastructure.Configurations;

public class MatchTeamConfiguration : IEntityTypeConfiguration<MatchTeam>
{
    public void Configure(EntityTypeBuilder<MatchTeam> builder)
    {
        builder.ToTable("Teams");

        // Composite key
        builder.HasKey(x => new
        {
            x.TeamId,
            x.MatchNo
        });

        builder.Property(x => x.TeamId)
            .ValueGeneratedNever();

        builder.Property(x => x.MatchNo)
            .ValueGeneratedNever();

        builder.Property(x => x.FullName)
            .HasMaxLength(100);

        builder.Property(x => x.ShortName)
            .HasMaxLength(50);

        builder.Property(x => x.Logo)
            .HasMaxLength(500);

        builder.Property(x => x.Scores)
            .HasMaxLength(50);

        builder.Property(x => x.MatchStatus)
            .HasMaxLength(50);

        builder.Property(x => x.Overs)
            .HasColumnType("decimal(18,2)");
    }
}