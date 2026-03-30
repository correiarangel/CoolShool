using CoolShool.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolShool.Infrastructure.Persistence.Configurations;

public class PaymentPlanConfiguration : IEntityTypeConfiguration<PaymentPlan>
{
    public void Configure(EntityTypeBuilder<PaymentPlan> builder)
    {
        builder.ToTable("payment_plan");
        builder.HasKey(p => p.Id);
        builder.Ignore(p => p.TotalAmount);

        builder.HasOne(p => p.FinancialOwner)
            .WithMany(f => f.PaymentPlans)
            .HasForeignKey(p => p.FinancialOwnerId);

        builder.HasOne(p => p.CostCenter)
            .WithMany(c => c.PaymentPlans)
            .HasForeignKey(p => p.CostCenterId);

        builder.HasMany(p => p.Billings)
           .WithOne(b => b.Plan)
           .HasForeignKey(b => b.PaymentPlanId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
