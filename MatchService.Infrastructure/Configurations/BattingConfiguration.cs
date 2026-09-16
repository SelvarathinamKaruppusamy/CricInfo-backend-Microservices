using MatchService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchService.Infrastructure.Configurations;

public class BattingConfiguration : IEntityTypeConfiguration<Batting>
{
    public void Configure(EntityTypeBuilder<Batting> builder)
    {
        builder.ToTable("Batting");

        builder.HasKey(x => new
        {
            x.TeamId,
            x.MatchNo,
            x.PlayerId
        });

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PlayerId)
            .ValueGeneratedNever();

        builder.Property(x => x.TeamId)
            .ValueGeneratedNever();

        builder.Property(x => x.MatchNo)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.Property(x => x.Role)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasMaxLength(50);

        builder.Property(x => x.StrikeRate)
            .HasColumnType("decimal(18,2)");
    }
}