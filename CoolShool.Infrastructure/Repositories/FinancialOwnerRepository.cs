using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;
using CoolShool.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoolShool.Infrastructure.Repositories;
public class FinancialOwnerRepository(AppDbContext context) : IFinancialOwnerRepository
{
    private readonly AppDbContext _context = context;

    public async Task<FinancialOwner?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.FinancialOwners
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<IEnumerable<FinancialOwner>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.FinancialOwners
            .ToListAsync(ct);
    }

    public async Task AddAsync(FinancialOwner owner, CancellationToken ct = default)
    {
        await _context.FinancialOwners.AddAsync(owner, ct);
    }

    public void Remove(FinancialOwner owner)
    {
        _context.FinancialOwners.Remove(owner);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
