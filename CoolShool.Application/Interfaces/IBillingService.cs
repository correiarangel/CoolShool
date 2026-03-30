using CoolShool.Application.Common;
using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;

namespace CoolShool.Application.Interfaces;

public interface IBillingService
{
    Task<Result<IEnumerable<BillingResponse>>> GetByOwnerAsync(long ownerId, CancellationToken ct = default);
    Task<Result<int>> GetCountByOwnerAsync(long ownerId, CancellationToken ct = default);
    Task<Result> RegisterPaymentAsync(long billingId, RegisterPaymentRequest request, CancellationToken ct = default);
    Task<Result> CancelAsync(long id, CancellationToken ct = default);
    Task<Result> DeleteAsync(long id, CancellationToken ct = default);
}
