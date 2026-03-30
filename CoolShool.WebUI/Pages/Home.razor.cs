using CoolShool.WebUI.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CoolShool.WebUI.Pages;

public partial class Home : ComponentBase
{
    [Inject] private ICoolSchoolClient Client { get; set; } = default!;

    private bool _isLoading = true;
    private string? _error;

    // KPIs
    private decimal _totalGeral;
    private decimal _totalRecebido;
    private decimal _totalPendente;
    private decimal _totalAtrasado;

    // Chart
    private List<ChartSeries<double>> _chartSeries = [];
    private string[] _chartLabels = [];

    // Upcoming
    private List<UpcomingBillingModel> _upcomingBillings = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var result = await Client.GetDashboardData.ExecuteAsync();

            if (result.Errors.Any())
            {
                _error = "Erro ao carregar dados do Dashboard.";
            }
            else if (result.Data != null)
            {
                var data = result.Data;
                var allBillings = data.PaymentPlans.SelectMany(p => p.Billings).ToList();

                // 1. KPIs
                _totalGeral = allBillings.Sum(b => b.Amount);
                _totalRecebido = allBillings.Where(b => b.Status == BillingStatus.Paid).Sum(b => b.Amount);

                var issued = allBillings.Where(b => b.Status == BillingStatus.Issued).ToList();
                _totalPendente = issued.Where(b => b.DueDate.Date >= DateTime.Today).Sum(b => b.Amount);
                _totalAtrasado = issued.Where(b => b.DueDate.Date < DateTime.Today).Sum(b => b.Amount);

                // 2. Gráfico por Centro de Custo
                var costCenterNames = data.CostCenters.ToDictionary(cc => cc.Id, cc => cc.DisplayName);
                var plansByCostCenter = data.PaymentPlans
                    .GroupBy(p => p.CostCenterId)
                    .Select(g => new
                    {
                        Label = costCenterNames.GetValueOrDefault(g.Key) ?? "Desconhecido",
                        Value = (double)g.Sum(p => p.TotalAmount)
                    })
                    .ToList();

                _chartSeries =
                [
                    new ChartSeries<double> { Name = "Distribuição", Data = plansByCostCenter.Select(x => x.Value).ToArray() }
                ];
                _chartLabels = plansByCostCenter.Select(x => x.Label).ToArray();

                // 3. Próximos Vencimentos
                var owners = data.FinancialOwners.ToDictionary(o => o.Id, o => o.Name);
                _upcomingBillings = data.PaymentPlans
                    .SelectMany(p => p.Billings.Select(b => new UpcomingBillingModel
                    {
                        Id = b.Id,
                        Amount = b.Amount,
                        DueDate = b.DueDate,
                        Status = b.Status,
                        OwnerName = owners.GetValueOrDefault(p.FinancialOwnerId) ?? "N/A"
                    }))
                    .Where(b => b.Status == BillingStatus.Issued)
                    .OrderBy(b => b.DueDate)
                    .Take(5)
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            _error = $"Exceção ao carregar dados: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }
}
