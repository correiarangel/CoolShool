namespace CoolShool.WebUI.Models;

/// <summary>
/// Model de apresentação para um Plano de Pagamento na listagem da UI.
/// Representa os dados já carregados e mapeados do servidor, prontos para renderização.
/// </summary>
public sealed class PlanModel
{
    public long Id { get; init; }
    public decimal TotalAmount { get; init; }
    public long FinancialOwnerId { get; init; }
    public long CostCenterId { get; init; }
    public string OwnerName { get; init; } = string.Empty;
    public string CostCenterDisplayName { get; init; } = string.Empty;
    public IReadOnlyList<PlanBillingModel> Billings { get; init; } = [];
}

/// <summary>
/// Model de apresentação para uma parcela dentro de um Plano de Pagamento.
/// </summary>
public sealed class PlanBillingModel
{
    public long Id { get; init; }
    public decimal Amount { get; init; }
    public DateTimeOffset DueDate { get; init; }
    public BillingStatus Status { get; init; }
    public PaymentType PaymentMethod { get; init; }
}
