using CoolShool.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolShool.Infrastructure.Persistence.Configurations;

public class BillingConfiguration : IEntityTypeConfiguration<Billing>
{
    public void Configure(EntityTypeBuilder<Billing> builder)
    {
        builder.ToTable("billing");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Amount).HasPrecision(18, 2).IsRequired();
        builder.Property(b => b.PaymentCode).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Status).IsRequired();
        builder.Property(b => b.PaymentMethod).IsRequired();

        builder.Ignore(b => b.IsOverdue);

        builder.HasOne(b => b.Plan).WithMany(b => b.Billings).HasForeignKey(b => b.PaymentPlanId);
    }
}
