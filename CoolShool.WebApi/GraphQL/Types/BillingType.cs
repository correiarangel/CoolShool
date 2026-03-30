using CoolShool.Domain.Models;
using HotChocolate.Types;

namespace CoolShool.WebApi.GraphQL.Types;

public class BillingType : ObjectType<Billing>
{
    protected override void Configure(IObjectTypeDescriptor<Billing> descriptor)
    {
        descriptor.Description("Representa uma cobrança individual ou parcela.");
        
        descriptor.Field(b => b.Id).Type<NonNullType<LongType>>();
        descriptor.Field(b => b.Amount).Type<NonNullType<DecimalType>>();
        descriptor.Field(b => b.DueDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(b => b.Status).Type<NonNullType<EnumType<CoolShool.Domain.Enums.BillingStatus>>>();
        descriptor.Field(b => b.PaymentMethod).Type<NonNullType<EnumType<CoolShool.Domain.Enums.PaymentType>>>();
        descriptor.Field(b => b.IsOverdue).Type<NonNullType<BooleanType>>();
    }
}
