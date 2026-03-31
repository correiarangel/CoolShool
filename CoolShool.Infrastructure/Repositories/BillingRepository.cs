using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolShool.Infrastructure.Repositories;

public class BillingRepository(AppDbContext context) : IBillingRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Billing>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Billings
        .Include(b => b.Plan)
        .ToListAsync(cancellationToken: ct);
    }

    public async Task<int> CountByOwnerAsync(long financialOwnerId, CancellationToken ct = default)
    {
        return await _context.Billings
        .CountAsync(b => b.Plan.FinancialOwnerId == financialOwnerId, cancellationToken: ct);
    }

    public async Task<Billing?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.Billings
        .Include(b => b.Payments)
        .FirstOrDefaultAsync(b => b.Id == id, cancellationToken: ct);
    }

    public async Task<IEnumerable<Billing>> GetByOwnerAsync(long financialOwnerId, CancellationToken ct = default)
    {
        return await _context.Billings
      .Include(b => b.Plan)
      .Where(b => b.Plan.FinancialOwnerId == financialOwnerId)
      .ToListAsync(cancellationToken: ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
