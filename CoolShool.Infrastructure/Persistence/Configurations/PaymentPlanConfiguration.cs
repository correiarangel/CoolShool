using CoolShool.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolShool.Infrastructure.Persistence.Configurations;

    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payment");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount).HasPrecision(18, 2).IsRequired();
            builder.Property(p => p.PaymentDate).IsRequired();

            //relação com conbrança
            builder.HasOne(p => p.Billing)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BillingId);
        }
    }
