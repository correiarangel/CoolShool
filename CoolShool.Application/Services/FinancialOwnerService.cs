using CoolShool.Application.Common;
using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;
using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;

namespace CoolShool.Application.Services;

public sealed class FinancialOwnerService(IFinancialOwnerRepository repository) : IFinancialOwnerService
{
    public async Task<Result<FinancialOwnerResponse>> CreateAsync(CreateFinancialOwnerRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<FinancialOwnerResponse>.Failure("O nome do responsável é obrigatório.");

        var owner = new FinancialOwner(request.Name);
        await repository.AddAsync(owner, ct);
        await repository.SaveChangesAsync(ct);
        
        return new FinancialOwnerResponse(owner.Id, owner.Name);
    }

    public async Task<Result<IEnumerable<FinancialOwnerResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var owners = await repository.GetAllAsync(ct);
        return Result<IEnumerable<FinancialOwnerResponse>>.Success(owners.Select(o => new FinancialOwnerResponse(o.Id, o.Name)));
    }

    public async Task<Result<FinancialOwnerResponse>> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var owner = await repository.GetByIdAsync(id, ct);
        if (owner == null)
            return Result<FinancialOwnerResponse>.Failure("Responsável financeiro não encontrado.");
            
        return new FinancialOwnerResponse(owner.Id, owner.Name);
    }

    public async Task<Result<FinancialOwnerResponse>> UpdateAsync(long id, UpdateFinancialOwnerRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<FinancialOwnerResponse>.Failure("O nome do responsável é obrigatório.");

        var owner = await repository.GetByIdAsync(id, ct);
        if (owner == null)
            return Result<FinancialOwnerResponse>.Failure("Responsável financeiro não encontrado.");

        owner.Update(request.Name);
        await repository.SaveChangesAsync(ct);

        return new FinancialOwnerResponse(owner.Id, owner.Name);
    }

    public async Task<Result> DeleteAsync(long id, CancellationToken ct = default)
    {
        var owner = await repository.GetByIdAsync(id, ct);
        if (owner == null)
            return Result.Failure("Responsável financeiro não encontrado.");

        if (owner.PaymentPlans.Any())
            return Result.Failure("Não é permitido excluir um responsável que possui planos de pagamento vinculados.");

        repository.Remove(owner);
        await repository.SaveChangesAsync(ct);

        return Result.Success();
    }
}
