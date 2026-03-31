using CoolShool.Domain.Enums;

namespace CoolShool.Domain.Models;

public class PaymentPlan
{
    public long Id { get; private set; }
    public long FinancialOwnerId { get; private set; }
    public FinancialOwner FinancialOwner { get; private set; } = null!;
    public long CostCenterId { get; private set; }
    public CostCenter CostCenter { get; private set; } = null!;

    private readonly List<Billing> _billings = [];
    public IReadOnlyCollection<Billing> Billings => _billings;

    /// <summary>Total calculado — NÃO persistido. Soma dos valores de todas as cobranças.</summary>
    public decimal TotalAmount => _billings.Sum(b => b.Amount);

    protected PaymentPlan() { }

    public PaymentPlan(long financialOwnerId, long costCenterId)
    {
        FinancialOwnerId = financialOwnerId;
        CostCenterId = costCenterId;
    }

    /// <summary>
    /// Comportamento de domínio 
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="dueDate"></param>
    /// <param name="paymentMethod"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddBilling(decimal amount, DateTime dueDate, PaymentType paymentMethod)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor da cobrança deve ser maior que zero.", nameof(amount));

        var billing = new Billing(amount, dueDate, paymentMethod);
        billing.AssignToPaymentPlan(this);
        _billings.Add(billing);
    }
}
