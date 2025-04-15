using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ExternalCustomerConfiguration : IEntityTypeConfiguration<ExternalCustomer>
{
    public void Configure(EntityTypeBuilder<ExternalCustomer> builder)
    {
        builder.ToTable("ExternalCustomers");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.CustomerName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(15);
        
        builder.Property(s => s.IsActive)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnType("timestamp");
    }
}
