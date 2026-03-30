using CoolShool.WebUI.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CoolShool.WebUI.Pages;

public partial class CreatePaymentPlanDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Inject] private ICoolSchoolClient Client { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private List<IGetFinancialOwners_FinancialOwners> _owners = [];
    private List<IGetDashboardData_CostCenters> _centers = [];

    private long _selectedOwnerId;
    private long _selectedCenterId;
    private List<BillingEntryModel> _billings = [];
    private MudForm _form = default!;
    private bool _formValid;
    private bool _processing;

    private bool CanSubmit =>
        _formValid &&
        _selectedOwnerId > 0 &&
        _selectedCenterId > 0 &&
        _billings.Count > 0 &&
        _billings.All(b => b.Amount > 0 && b.DueDateNullable.HasValue);

    protected override async Task OnInitializedAsync()
    {
        var ownersTask = Client.GetFinancialOwners.ExecuteAsync();
        var centersTask = Client.GetDashboardData.ExecuteAsync();

        await Task.WhenAll(ownersTask, centersTask);

        var ownersResult = await ownersTask;
        if (ownersResult.Data?.FinancialOwners != null)
            _owners = ownersResult.Data.FinancialOwners.ToList();

        var centersResult = await centersTask;
        if (centersResult.Data?.CostCenters != null)
            _centers = centersResult.Data.CostCenters.ToList();

        AddBilling();
    }

    private void AddBilling()
    {
        var nextDueDate = _billings.Count == 0
            ? DateTime.Today.AddMonths(1)
            : (_billings.Last().DueDateNullable?.AddMonths(1) ?? DateTime.Today.AddMonths(1));

        _billings.Add(new BillingEntryModel { DueDateNullable = nextDueDate });
    }

    private void RemoveBilling(int index)
    {
        if (_billings.Count > 1)
            _billings.RemoveAt(index);
    }

    private void Cancel() => MudDialog.Cancel();

    private async Task Submit()
    {
        await _form.ValidateAsync();
        if (!CanSubmit) return;

        _processing = true;
        try
        {
            var billingInputs = _billings.Select(b => new CreateBillingRequestInput
            {
                Amount = b.Amount,
                DueDate = b.DueDateNullable!.Value,
                PaymentMethod = b.Method
            }).ToList();

            var result = await Client.CreatePaymentPlan.ExecuteAsync(
                _selectedOwnerId,
                _selectedCenterId,
                billingInputs);

            if (result.Errors.Any())
                Snackbar.Add(result.Errors.First().Message, Severity.Error);
            else
            {
                var total = result.Data?.CreatePaymentPlan?.TotalAmount ?? 0;
                var count = result.Data?.CreatePaymentPlan?.Billings.Count ?? 0;
                Snackbar.Add($"Plano criado com {count} parcela(s)! Total: {total:C}", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao criar plano: {ex.Message}", Severity.Error);
        }
        finally
        {
            _processing = false;
        }
    }
}
