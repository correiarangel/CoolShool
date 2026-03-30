namespace CoolShool.WebUI.Models;

/// <summary>
/// Model mutável que representa uma entrada de parcela durante a criação de um Plano de Pagamento.
/// É utilizado somente no formulário da UI e não persiste no servidor diretamente.
/// </summary>
public sealed class BillingEntryModel
{
    public decimal Amount { get; set; } = 0;
    public DateTime? DueDateNullable { get; set; } = DateTime.Today.AddMonths(1);
    public PaymentType Method { get; set; } = PaymentType.Boleto;
}
