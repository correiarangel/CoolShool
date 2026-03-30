using CoolShool.Application.Common;
using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;
using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;

namespace CoolShool.Application.Services;

public sealed class PaymentPlanService(IPaymentPlanRepository repository) : IPaymentPlanService
{
    public async Task<Result<PaymentPlanResponse>> CreateAsync(CreatePaymentPlanRequest request, CancellationToken ct = default)
    {
        var plan = new PaymentPlan(request.FinancialOwnerId, request.CostCenterId);
        
        foreach (var b in request.Billings)
        {
            plan.AddBilling(b.Amount, b.DueDate, b.PaymentMethod);
        }

        await repository.AddAsync(plan, ct);
        await repository.SaveChangesAsync(ct);

        return MapToResponse(plan);
    }

    public async Task<Result<PaymentPlanResponse>> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var plan = await repository.GetByIdAsync(id, ct);
        if (plan == null) 
            return Result<PaymentPlanResponse>.Failure("Plano de pagamento não encontrado.");

        return MapToResponse(plan);
    }

    public async Task<Result<IEnumerable<PaymentPlanResponse>>> GetByOwnerAsync(long ownerId, CancellationToken ct = default)
    {
        var plans = await repository.GetByOwnerAsync(ownerId, ct);
        return Result<IEnumerable<PaymentPlanResponse>>.Success(plans.Select(MapToResponse));
    }

    public async Task<Result> DeleteAsync(long id, CancellationToken ct = default)
    {
        var plan = await repository.GetByIdAsync(id, ct);
        if (plan == null)
            return Result.Failure("Plano de pagamento não encontrado.");

        repository.Remove(plan);
        await repository.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static PaymentPlanResponse MapToResponse(PaymentPlan plan)
    {
        return new PaymentPlanResponse(
            plan.Id,
            plan.FinancialOwnerId,
            plan.CostCenterId,
            plan.TotalAmount,
            plan.Billings.Select(b => new BillingResponse(
                b.Id,
                b.Amount,
                b.DueDate,
                b.PaymentMethod,
                b.Status,
                b.PaymentCode,
                b.IsOverdue
            ))
        );
    }
}
