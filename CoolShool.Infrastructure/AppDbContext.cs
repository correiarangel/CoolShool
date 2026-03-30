using CoolShool.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolShool.Infrastructure
{
    
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
       
        public DbSet<FinancialOwner> FinancialOwners { get; set; }
        public DbSet<PaymentPlan> PaymentPlans { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
