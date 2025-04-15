using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ExternalBranchConfiguration : IEntityTypeConfiguration<ExternalBranch>
{
    public void Configure(EntityTypeBuilder<ExternalBranch> builder)
    {
        builder.ToTable("ExternalBranches");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(b => b.BranchName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(b => b.Location)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(s => s.IsActive)
                    .IsRequired();

        builder.Property(b => b.CreatedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(b => b.UpdatedAt)
            .HasColumnType("timestamp");
    }
}