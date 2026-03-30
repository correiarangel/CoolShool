using CoolShool.Domain.Enums;
using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;
using CoolShool.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoolShool.Infrastructure.Repositories;

public class CostCenterRepository(AppDbContext context) : ICostCenterRepository
{
    private readonly AppDbContext _context = context;
    public async Task AddAsync(CostCenter costCenter, CancellationToken ct = default)
    {
        await _context.CostCenters.AddAsync(costCenter, ct);
    }

    public void Remove(CostCenter costCenter)
    {
        _context.CostCenters.Remove(costCenter);
    }

    public async Task<IEnumerable<CostCenter>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.CostCenters.ToListAsync(cancellationToken: ct);
    }

    public async Task<CostCenter?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.CostCenters.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<CostCenter?> GetByTypeAsync(CostCenterType type, CancellationToken ct = default)
    {
        return await _context.CostCenters.FirstOrDefaultAsync(c => c.Type == type, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }

    public async Task<CostCenter?> GetByTypesAsync(CostCenterType type, CancellationToken ct = default)
    {
        return await _context.CostCenters.FirstOrDefaultAsync(c => c.Type == type, ct);
    }
}
