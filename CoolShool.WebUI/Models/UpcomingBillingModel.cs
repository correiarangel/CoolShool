namespace CoolShool.WebUI.Models;

/// <summary>
/// Model de apresentação para as cobranças próximas do vencimento no Dashboard.
/// </summary>
public sealed class UpcomingBillingModel
{
    public long Id { get; init; }
    public decimal Amount { get; init; }
    public DateTimeOffset DueDate { get; init; }
    public BillingStatus Status { get; init; }
    public string OwnerName { get; init; } = string.Empty;
}
