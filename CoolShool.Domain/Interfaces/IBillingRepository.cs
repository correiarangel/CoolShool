using CoolShool.Domain.Models;
namespace CoolShool.Domain.Interfaces;

public interface IBillingRepository
{
    Task<Billing?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<Billing>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Billing>> GetByOwnerAsync(long financialOwnerId, CancellationToken ct = default);
    Task<int> CountByOwnerAsync(long financialOwnerId, CancellationToken ct = default);
    void Remove(Billing billing);
    Task SaveChangesAsync(CancellationToken ct = default);

}
