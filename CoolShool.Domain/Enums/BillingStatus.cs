namespace CoolShool.Domain.Enums;

/// <summary>Status persistido da cobrança.</summary>
public enum BillingStatus
{
    ISSUED    = 0,  // Emitida (status inicial)
    PAID      = 1,  // Paga
    CANCELLED = 2,  // Cancelada
}
