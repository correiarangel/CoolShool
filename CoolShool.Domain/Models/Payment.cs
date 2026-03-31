namespace CoolShool.Domain.Models;

public class Payment
{
    public long Id { get; private set; }
    public long BillingId { get; private set; }
    public Billing Billing { get; private set; } = null!;
    public DateTime PaymentDate { get; private set; }
    public decimal Amount { get; private set; }

    protected Payment() { }

    /// <param name="amount">Valor pago.</param>
    /// <param name="paymentDate">Data do pagamento; usa UtcNow quando não informada.</param>
    public Payment(decimal amount, DateTime? paymentDate = null)
    {
        Amount = amount;
        PaymentDate = paymentDate ?? DateTime.UtcNow;
    }

    internal void AssignToBilling(Billing billing)
    {
        BillingId = billing.Id;
        Billing = billing;
    }
}
