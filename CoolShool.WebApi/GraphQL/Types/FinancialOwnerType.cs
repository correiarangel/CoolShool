using CoolShool.Domain.Models;
using CoolShool.WebApi.GraphQL.DataLoaders;
using HotChocolate.Types;

namespace CoolShool.WebApi.GraphQL.Types;

public class FinancialOwnerType : ObjectType<FinancialOwner>
{
    protected override void Configure(IObjectTypeDescriptor<FinancialOwner> descriptor)
    {
        descriptor.Description("Representa o responsável financeiro pelos planos de pagamento.");
        
        descriptor.Field(f => f.Id).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.Name).Type<NonNullType<StringType>>();
        
        descriptor.Field(f => f.PaymentPlans)
            .Description("Lista de planos de pagamento associados a este responsável.")
            .ResolveWith<FinancialOwnerResolvers>(r => r.GetPaymentPlansAsync(default!, default!, default));
    }

    private class FinancialOwnerResolvers
    {
        public async Task<IEnumerable<PaymentPlan>> GetPaymentPlansAsync(
            [Parent] FinancialOwner owner,
            PaymentPlanBatchDataLoader dataLoader,
            CancellationToken ct)
        {
            return await dataLoader.LoadAsync(owner.Id, ct) ?? [];
        }
    }
}
