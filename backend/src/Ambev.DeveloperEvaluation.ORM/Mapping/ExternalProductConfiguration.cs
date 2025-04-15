using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ExternalProductConfiguration : IEntityTypeConfiguration<ExternalProduct>
{
    public void Configure(EntityTypeBuilder<ExternalProduct> builder)
    {
        builder.ToTable("ExternalProducts");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.ProductName)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.IsActive)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("timestamp");
    }
}