using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;
using CoolShool.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoolShool.Infrastructure.Repositories;

public class PaymentPlanRepository(AppDbContext context) : IPaymentPlanRepository
{
    private readonly AppDbContext _context = context;
    public async Task AddAsync(PaymentPlan paymentPlan, CancellationToken ct = default)
    {
        await _context.PaymentPlans.AddAsync(paymentPlan, ct);
    }

    public async Task<IEnumerable<PaymentPlan>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.PaymentPlans
            .Include(p => p.Billings)
            .Include(p => p.CostCenter)
            .Include(p => p.FinancialOwner)
            .ToArrayAsync(ct);
    }

    public void Remove(PaymentPlan paymentPlan)
    {
        _context.PaymentPlans.Remove(paymentPlan);
    }

    public async Task<PaymentPlan?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.PaymentPlans
            .Include(p => p.Billings)
            .Include(p => p.CostCenter)
            .Include(p => p.FinancialOwner)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IEnumerable<PaymentPlan>> GetByOwnerAsync(long financialOwnerId, CancellationToken ct = default)
    {
        return await _context.PaymentPlans
            .Include(p => p.Billings)
            .Include(p => p.CostCenter)
            .Include(p => p.FinancialOwner)
            .Where(p => p.FinancialOwnerId == financialOwnerId)
            .ToArrayAsync(ct);
    }

    public async Task<IEnumerable<PaymentPlan>> GetByOwnerIdsAsync(IEnumerable<long> ownerIds, CancellationToken ct = default)
    {
        return await _context.PaymentPlans
            .Include(p => p.Billings)
            .Include(p => p.CostCenter)
            .Include(p => p.FinancialOwner)
            .Where(p => ownerIds.Contains(p.FinancialOwnerId))
            .ToArrayAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
