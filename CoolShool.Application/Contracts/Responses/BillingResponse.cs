using CoolShool.Domain.Enums;

namespace CoolShool.Application.Contracts.Responses;

public sealed record BillingResponse(
    long Id,
    decimal Amount,
    DateTime DueDate,
    PaymentType PaymentMethod,
    BillingStatus Status,
    string PaymentCode,
    bool IsOverdue
);
