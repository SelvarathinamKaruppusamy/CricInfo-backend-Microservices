using MatchService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchService.Infrastructure.Configurations;

public class BowlingConfiguration : IEntityTypeConfiguration<Bowling>
{
    public void Configure(EntityTypeBuilder<Bowling> builder)
    {
        builder.ToTable("Bowling");

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

        builder.Property(x => x.Overs)
            .HasMaxLength(20);

        builder.Property(x => x.Economy)
            .HasColumnType("decimal(18,2)");
    }
}