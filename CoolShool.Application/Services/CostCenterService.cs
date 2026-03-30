using CoolShool.Application.Common;
using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;
using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;

namespace CoolShool.Application.Services;

public sealed class CostCenterService(ICostCenterRepository repository) : ICostCenterService
{
    public async Task<Result<CostCenterResponse>> CreateAsync(CreateCostCenterRequest request, CancellationToken ct = default)
    {
        var costCenter = request.Name != null 
            ? new CostCenter(request.Type, request.Name)
            : new CostCenter(request.Type);
            
        await repository.AddAsync(costCenter, ct);
        await repository.SaveChangesAsync(ct);
        
        return new CostCenterResponse(costCenter.Id, costCenter.Type, costCenter.Name, costCenter.DisplayName);
    }

    public async Task<Result<IEnumerable<CostCenterResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var centers = await repository.GetAllAsync(ct);
        return Result<IEnumerable<CostCenterResponse>>.Success(centers.Select(c => new CostCenterResponse(c.Id, c.Type, c.Name, c.DisplayName)));
    }

    public async Task<Result<CostCenterResponse>> UpdateAsync(long id, UpdateCostCenterRequest request, CancellationToken ct = default)
    {
        var costCenter = await repository.GetByIdAsync(id, ct);
        if (costCenter == null)
            return Result<CostCenterResponse>.Failure("Centro de custo não encontrado.");

        costCenter.Update(request.Type, request.Name);
        await repository.SaveChangesAsync(ct);

        return new CostCenterResponse(costCenter.Id, costCenter.Type, costCenter.Name, costCenter.DisplayName);
    }

    public async Task<Result> DeleteAsync(long id, CancellationToken ct = default)
    {
        var costCenter = await repository.GetByIdAsync(id, ct);
        if (costCenter == null)
            return Result.Failure("Centro de custo não encontrado.");

        if (costCenter.PaymentPlans.Any())
            return Result.Failure("Não é permitido excluir um centro de custo que possui planos de pagamento vinculados.");

        repository.Remove(costCenter);
        await repository.SaveChangesAsync(ct);

        return Result.Success();
    }
}
