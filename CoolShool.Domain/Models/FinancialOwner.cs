namespace CoolShool.Domain.Models;

public class FinancialOwner
{
    public long Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private readonly List<PaymentPlan> _paymentPlans = [];
    public IReadOnlyCollection<PaymentPlan> PaymentPlans => _paymentPlans;

    protected FinancialOwner() { }

    public FinancialOwner(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do responsável financeiro é obrigatório.", nameof(name));

        Name = name;
    }

    public void Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do responsável financeiro é obrigatório.", nameof(name));

        Name = name;
    }
}
