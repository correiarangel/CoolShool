using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;

namespace CoolShool.WebApi.GraphQL.DataLoaders;

public class PaymentPlanBatchDataLoader(
    IPaymentPlanRepository repository,
    IBatchScheduler batchScheduler,
    DataLoaderOptions? options = null)
    : BatchDataLoader<long, IEnumerable<PaymentPlan>>(batchScheduler, options ?? new DataLoaderOptions())
{
    private readonly IPaymentPlanRepository _repository = repository;

    protected override async Task<IReadOnlyDictionary<long, IEnumerable<PaymentPlan>>> LoadBatchAsync(
        IReadOnlyList<long> keys, 
        CancellationToken cancellationToken)
    {
        // Busca todos os planos para os IDs de proprietários fornecidos (IN)
        var plans = await _repository.GetByOwnerIdsAsync(keys, cancellationToken);
        
        // Agrupa os resultados por proprietário para retornar ao DataLoader
        return plans
            .GroupBy(p => p.FinancialOwnerId)
            .ToDictionary(g => g.Key, g => g.AsEnumerable());
    }
}
