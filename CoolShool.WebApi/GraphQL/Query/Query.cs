using CoolShool.Domain.Interfaces;
using CoolShool.Domain.Models;

namespace CoolShool.WebApi.GraphQL;

public class Query
{
    public Task<IEnumerable<FinancialOwner>> GetFinancialOwners([Service] IFinancialOwnerRepository repository) =>
        repository.GetAllAsync();

    public Task<FinancialOwner?> GetFinancialOwnerById(long id, [Service] IFinancialOwnerRepository repository) =>
        repository.GetByIdAsync(id);

    public Task<IEnumerable<PaymentPlan>> GetPaymentPlans([Service] IPaymentPlanRepository repository) =>
        repository.GetAllAsync();

    public Task<IEnumerable<CostCenter>> GetCostCenters([Service] ICostCenterRepository repository) =>
        repository.GetAllAsync();

    public Task<IEnumerable<Billing>> GetBillings([Service] IBillingRepository repository) =>
        repository.GetAllAsync();

    public Task<IEnumerable<Billing>> GetBillingsByOwner(long ownerId, [Service] IBillingRepository repository) =>
        repository.GetByOwnerAsync(ownerId);

    public Task<int> GetBillingCountByOwner(long ownerId, [Service] IBillingRepository repository) =>
        repository.CountByOwnerAsync(ownerId);
}
