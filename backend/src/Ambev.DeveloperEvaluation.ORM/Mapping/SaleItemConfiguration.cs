using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(si => si.Quantity)
            .IsRequired();
        
        builder.Property(si => si.Discount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.Property(s => s.IsCancelled)
            .IsRequired();

        builder.Property(si => si.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(si => si.CreatedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(si => si.UpdatedAt)
            .HasColumnType("timestamp");

        builder.HasOne(si => si.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey("SaleId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey("ProductId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}