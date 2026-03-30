namespace CoolShool.Application.Contracts.Requests;

public sealed record RegisterPaymentRequest(
    decimal Amount,
    DateTime? PaymentDate = null
);
