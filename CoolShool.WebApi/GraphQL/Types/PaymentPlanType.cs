using CoolShool.Domain.Models;
using HotChocolate.Types;

namespace CoolShool.WebApi.GraphQL.Types;

public class PaymentPlanType : ObjectType<PaymentPlan>
{
    protected override void Configure(IObjectTypeDescriptor<PaymentPlan> descriptor)
    {
        descriptor.Description("Representa um plano de pagamento contendo várias cobranças.");
        
        descriptor.Field(p => p.Id).Type<NonNullType<LongType>>();
        descriptor.Field(p => p.FinancialOwnerId).Type<NonNullType<LongType>>();
        descriptor.Field(p => p.CostCenterId).Type<NonNullType<LongType>>();
        descriptor.Field(p => p.TotalAmount).Type<NonNullType<DecimalType>>();
        
        descriptor.Field(p => p.Billings)
            .Description("As cobranças (parcelas) deste plano.");
    }
}
