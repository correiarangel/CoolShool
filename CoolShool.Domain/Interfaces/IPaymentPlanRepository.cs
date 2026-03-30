using CoolShool.Domain.Models;

namespace CoolShool.Domain.Interfaces;

public interface IPaymentPlanRepository
{
    Task<PaymentPlan?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<PaymentPlan>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<PaymentPlan>> GetByOwnerAsync(long financialOwnerId, CancellationToken ct = default);
    Task<IEnumerable<PaymentPlan>> GetByOwnerIdsAsync(IEnumerable<long> ownerIds, CancellationToken ct = default);
    Task AddAsync(PaymentPlan plan, CancellationToken ct = default);
    void Remove(PaymentPlan plan);
    Task SaveChangesAsync(CancellationToken ct = default);
}
