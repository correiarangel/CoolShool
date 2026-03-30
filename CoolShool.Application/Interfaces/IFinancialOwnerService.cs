using CoolShool.Application.Common;
using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;

namespace CoolShool.Application.Interfaces;

public interface IFinancialOwnerService
{
    Task<Result<FinancialOwnerResponse>> CreateAsync(CreateFinancialOwnerRequest request, CancellationToken ct = default);
    Task<Result<IEnumerable<FinancialOwnerResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<FinancialOwnerResponse>> GetByIdAsync(long id, CancellationToken ct = default);
    Task<Result<FinancialOwnerResponse>> UpdateAsync(long id, UpdateFinancialOwnerRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(long id, CancellationToken ct = default);
}
