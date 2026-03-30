using CoolShool.Domain.Enums;

namespace CoolShool.Application.Contracts.Requests;

public sealed record CreatePaymentPlanRequest(
    long FinancialOwnerId,
    long CostCenterId,
    IEnumerable<CreateBillingRequest> Billings
);

public sealed record CreateBillingRequest(
    decimal Amount,
    DateTime DueDate,
    PaymentType PaymentMethod
);
