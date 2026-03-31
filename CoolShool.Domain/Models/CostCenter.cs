using CoolShool.Domain.Enums;

namespace CoolShool.Domain.Models;

public class CostCenter
{
    public long Id { get; private set; }

    /// <summary>Tipo padrão do centro de custo (enum).</summary>
    public CostCenterType Type { get; private set; }

    /// <summary>
    /// Nome customizado (diferencial/plus).
    /// Quando informado, representa um centro de custo criado via API.
    /// Quando nulo, usa o nome do enum Type.
    /// </summary>
    public string? Name { get; private set; }

    private readonly List<PaymentPlan> _paymentPlans = [];
    public IReadOnlyCollection<PaymentPlan> PaymentPlans => _paymentPlans;

    protected CostCenter() { }

    /// <summary>Centro de custo padrão via enum.</summary>
    public CostCenter(CostCenterType type)
    {
        if (!Enum.IsDefined(typeof(CostCenterType), type))
            throw new ArgumentException("Tipo de centro de custo inválido.", nameof(type));

        Type = type;
    }

    /// <summary>Centro de custo customizável (diferencial) com nome livre.</summary>
    public CostCenter(CostCenterType type, string name) : this(type)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do centro de custo customizável é obrigatório.", nameof(name));

        Name = name;
    }

    /// <summary>Nome de exibição: customizado ou nome do enum.</summary>
    public string DisplayName => Name ?? Type.ToString();

    public void Update(CostCenterType type, string? name)
    {
        if (!Enum.IsDefined(typeof(CostCenterType), type))
            throw new ArgumentException("Tipo de centro de custo inválido.", nameof(type));

        if (name != null && string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do centro de custo customizável não pode ser vazio.", nameof(name));

        Type = type;
        Name = name;
    }
}
