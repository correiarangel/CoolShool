using CoolShool.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CostCenterConfiguration : IEntityTypeConfiguration<CostCenter>
{
    public void Configure(EntityTypeBuilder<CostCenter> builder)
    {
        builder.ToTable("cost_center");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Type).IsRequired();
        builder.Property(c => c.Name).HasMaxLength(60).IsRequired();
        builder.Ignore(c => c.DisplayName);
    }
}