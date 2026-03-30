using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CoolShool.WebUI.Pages;

public partial class CostCenters : ComponentBase
{
    [Inject] private ICoolSchoolClient Client { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;

    private IEnumerable<IGetDashboardData_CostCenters> _centers = [];
    private bool _loading = true;
    private string _searchString = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadCenters();
    }

    private bool FilterFunc(IGetDashboardData_CostCenters element)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        return (element.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
               (element.DisplayName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
               element.Type.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
               element.Id.ToString().Contains(_searchString);
    }

    private async Task LoadCenters()
    {
        _loading = true;
        try
        {
            var result = await Client.GetDashboardData.ExecuteAsync();
            if (result.Data?.CostCenters != null)
                _centers = result.Data.CostCenters;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao carregar centros de custo: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OpenCreateDialog()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CostCenterDialog>("Novo Centro de Custo", new DialogParameters(), options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
            await LoadCenters();
    }

    private async Task OpenEditDialog(IGetDashboardData_CostCenters center)
    {
        var parameters = new DialogParameters
        {
            ["CenterId"] = center.Id,
            ["Type"] = center.Type,
            ["Name"] = center.Name
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CostCenterDialog>("Editar Centro de Custo", parameters, options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
            await LoadCenters();
    }

    private async Task ConfirmDelete(IGetDashboardData_CostCenters center)
    {
        var parameters = new DialogParameters
        {
            ["Title"] = "Confirmar Exclusão",
            ["Message"] = $"Deseja realmente excluir o centro de custo <b>{center.DisplayName}</b>?",
            ["ConfirmText"] = "Excluir",
            ["CancelText"] = "Cancelar"
        };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmar Exclusão", parameters);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            try
            {
                var response = await Client.DeleteCostCenter.ExecuteAsync(center.Id);
                if (response.Errors.Any())
                    Snackbar.Add(response.Errors.First().Message, Severity.Warning);
                else
                {
                    Snackbar.Add("Centro de custo excluído com sucesso!", Severity.Success);
                    await LoadCenters();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Erro ao excluir: {ex.Message}", Severity.Error);
            }
        }
    }
}
