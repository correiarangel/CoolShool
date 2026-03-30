using CoolShool.Domain.Models;

namespace CoolShool.Domain.Interfaces;

public interface IFinancialOwnerRepository
{
    Task<FinancialOwner?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<FinancialOwner>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(FinancialOwner owner, CancellationToken ct = default);
    void Remove(FinancialOwner owner);
    Task SaveChangesAsync(CancellationToken ct = default);
}
