using CoolShool.Domain.Enums;
using CoolShool.Domain.Models;

namespace CoolShool.Domain.Interfaces;

public interface ICostCenterRepository
{
    Task<CostCenter?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<CostCenter?> GetByTypeAsync(CostCenterType type, CancellationToken ct = default);
    Task<IEnumerable<CostCenter>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(CostCenter costCenter, CancellationToken ct = default);
    void Remove(CostCenter costCenter);
    Task SaveChangesAsync(CancellationToken ct = default);
}
