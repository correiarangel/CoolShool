using CoolShool.Application.Common;
using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;

namespace CoolShool.Application.Interfaces;

public interface IPaymentPlanService
{
    Task<Result<PaymentPlanResponse>> CreateAsync(CreatePaymentPlanRequest request, CancellationToken ct = default);
    Task<Result<PaymentPlanResponse>> GetByIdAsync(long id, CancellationToken ct = default);
    Task<Result<IEnumerable<PaymentPlanResponse>>> GetByOwnerAsync(long ownerId, CancellationToken ct = default);
    Task<Result> DeleteAsync(long id, CancellationToken ct = default);
}