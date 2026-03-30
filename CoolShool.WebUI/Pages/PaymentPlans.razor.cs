using CoolShool.WebUI.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CoolShool.WebUI.Pages;

public partial class PaymentPlans : ComponentBase
{
    [Inject] private ICoolSchoolClient Client { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;

    private List<PlanModel> _plans = [];
    private bool _loading = true;
    private string _searchString = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadPlans();
    }

    private bool FilterFunc(PlanModel plan)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        return plan.Id.ToString().Contains(_searchString) ||
               plan.OwnerName.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
               plan.CostCenterDisplayName.Contains(_searchString, StringComparison.OrdinalIgnoreCase);
    }

    private async Task LoadPlans()
    {
        _loading = true;
        StateHasChanged();
        try
        {
            var result = await Client.GetPaymentPlans.ExecuteAsync();
            
            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    Snackbar.Add($"Erro GraphQL: {error.Message}", Severity.Error);
                }
            }

            if (result.Data?.PaymentPlans != null)
            {
                _plans = result.Data.PaymentPlans.Select(p => new PlanModel
                {
                    Id = p.Id,
                    TotalAmount = p.TotalAmount,
                    FinancialOwnerId = p.FinancialOwnerId,
                    CostCenterId = p.CostCenterId,
                    OwnerName = p.FinancialOwner?.Name ?? string.Empty,
                    CostCenterDisplayName = p.CostCenter?.DisplayName ?? string.Empty,
                    Billings = p.Billings.Select(b => new PlanBillingModel
                    {
                        Id = b.Id,
                        Amount = b.Amount,
                        DueDate = b.DueDate,
                        Status = b.Status,
                        PaymentMethod = b.PaymentMethod
                    }).ToList()
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao carregar planos: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    private async Task OpenCreateDialog()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CreatePaymentPlanDialog>(
            "Novo Plano de Pagamento", new DialogParameters(), options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
            await LoadPlans();
    }
}
