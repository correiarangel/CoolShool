using CoolShool.Application.Common;
using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;
using CoolShool.Domain.Interfaces;

namespace CoolShool.Application.Services;

public sealed class BillingService(IBillingRepository repository) : IBillingService
{
    public async Task<Result<IEnumerable<BillingResponse>>> GetByOwnerAsync(long ownerId, CancellationToken ct = default)
    {
        var billings = await repository.GetByOwnerAsync(ownerId, ct);
        return Result<IEnumerable<BillingResponse>>.Success(billings.Select(b => new BillingResponse(
            b.Id,
            b.Amount,
            b.DueDate,
            b.PaymentMethod,
            b.Status,
            b.PaymentCode,
            b.IsOverdue
        )));
    }

    public async Task<Result<int>> GetCountByOwnerAsync(long ownerId, CancellationToken ct = default)
    {
        var count = await repository.CountByOwnerAsync(ownerId, ct);
        return Result<int>.Success(count);
    }

    public async Task<Result> RegisterPaymentAsync(long billingId, RegisterPaymentRequest request, CancellationToken ct = default)
    {
        var billing = await repository.GetByIdAsync(billingId, ct);
        if (billing == null) 
            return Result.Failure("Cobrança não encontrada.");

        try 
        {
            billing.RegisterPayment(request.Amount, request.PaymentDate);
            await repository.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> CancelAsync(long id, CancellationToken ct = default)
    {
        var billing = await repository.GetByIdAsync(id, ct);
        if (billing == null)
            return Result.Failure("Cobrança não encontrada.");

        try 
        {
            billing.Cancel();
            await repository.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(long id, CancellationToken ct = default)
    {
        var billing = await repository.GetByIdAsync(id, ct);
        if (billing == null)
            return Result.Failure("Cobrança não encontrada.");

        repository.Remove(billing);
        await repository.SaveChangesAsync(ct);

        return Result.Success();
    }
}
