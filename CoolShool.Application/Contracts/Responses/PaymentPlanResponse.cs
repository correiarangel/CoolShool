namespace CoolShool.Application.Contracts.Responses;

public sealed record PaymentPlanResponse(
    long Id,
    long FinancialOwnerId,
    long CostCenterId,
    decimal TotalAmount,
    IEnumerable<BillingResponse> Billings
);
