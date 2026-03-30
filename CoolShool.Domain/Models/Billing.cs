using CoolShool.Domain.Enums;

namespace CoolShool.Domain.Models;

public class Billing
{
    public long Id { get; private set; }
    public long PaymentPlanId { get; private set; }
    public PaymentPlan Plan { get; private set; } = null!;
    public DateTime DueDate { get; private set; }
    public PaymentType PaymentMethod { get; private set; }
    public BillingStatus Status { get; private set; }
    public decimal Amount { get; private set; }
    public string PaymentCode { get; private set; } = string.Empty;

    private readonly List<Payment> _payments = [];
    public IReadOnlyCollection<Payment> Payments => _payments;

    /// <summary>EF Core: construtor protegido para materialização.</summary>
    protected Billing() { }

    public Billing(decimal amount, DateTime dueDate, PaymentType paymentMethod)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor da cobrança deve ser maior que zero.", nameof(amount));

        Amount = amount;
        DueDate = dueDate;
        PaymentMethod = paymentMethod;
        Status = BillingStatus.ISSUED;
        PaymentCode = GeneratePaymentCode(paymentMethod);
    }

    // ────────────────────── Comportamento de domínio ──────────────────────

    public void RegisterPayment(decimal amount, DateTime? paymentDate = null)
    {
        if (Status == BillingStatus.CANCELLED)
            throw new InvalidOperationException("Não é possível registrar pagamento em uma cobrança CANCELADA.");

        if (Status == BillingStatus.PAID)
            throw new InvalidOperationException("Esta cobrança já foi PAGA.");

        if (amount <= 0)
            throw new ArgumentException("O valor do pagamento deve ser maior que zero.", nameof(amount));

        var payment = new Payment(amount, paymentDate ?? DateTime.UtcNow);
        payment.AssignToBilling(this);

        _payments.Add(payment);
        Status = BillingStatus.PAID;
    }

    public void Cancel()
    {
        if (Status == BillingStatus.PAID)
            throw new InvalidOperationException("Não é possível cancelar uma cobrança que já foi PAGA.");

        Status = BillingStatus.CANCELLED;
    }

    /// <summary>
    /// Status VENCIDA — derivado, NÃO persistido.
    /// Regra: data atual > DueDate e cobrança não está PAGA nem CANCELADA.
    /// </summary>
    public bool IsOverdue =>
        Status == BillingStatus.ISSUED && DateTime.UtcNow.Date > DueDate.Date;

    // ────────────────────── Linkage interno (EF navigation) ──────────────────────

    internal void AssignToPaymentPlan(PaymentPlan plan)
    {
        PaymentPlanId = plan.Id;
        Plan = plan;
    }

    // ────────────────────── Helpers privados ──────────────────────

    private static string GeneratePaymentCode(PaymentType method) => method switch
    {
        PaymentType.BOLETO => GenerateBoletoCode(),
        PaymentType.PIX    => GeneratePixCode(),
        _                  => string.Empty
    };

    /// <summary>BOLETO: linha digitável simulada — 23 caracteres numéricos.</summary>
    private static string GenerateBoletoCode()
    {
        var digits = new string(Guid.NewGuid().ToString("N").Where(char.IsDigit).ToArray());
        return digits.PadRight(23, '0')[..23];
    }

    /// <summary>PIX: código simulado — GUID em Base64.</summary>
    private static string GeneratePixCode() =>
        Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}
