using CoolShool.Application.Common;
using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;

namespace CoolShool.Application.Interfaces;

public interface ICostCenterService
{
    Task<Result<CostCenterResponse>> CreateAsync(CreateCostCenterRequest request, CancellationToken ct = default);
    Task<Result<IEnumerable<CostCenterResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<CostCenterResponse>> UpdateAsync(long id, UpdateCostCenterRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(long id, CancellationToken ct = default);
}
