using CoolShool.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolShool.Infrastructure.Persistence.Configurations;

public class FinancialOwnerConfiguration : IEntityTypeConfiguration<FinancialOwner>
{
    public void Configure(EntityTypeBuilder<FinancialOwner> builder)
    {
        builder.ToTable("financial_owner");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(f => f.PaymentPlans)
            .WithOne(p => p.FinancialOwner)
            .HasForeignKey(p => p.FinancialOwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
