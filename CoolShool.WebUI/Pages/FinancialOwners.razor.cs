using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CoolShool.WebUI.Pages;

public partial class FinancialOwners : ComponentBase
{
    [Inject] private ICoolSchoolClient Client { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;

    private IEnumerable<IGetFinancialOwners_FinancialOwners> _owners = [];
    private bool _loading = true;
    private string _searchString = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadOwners();
    }

    private bool FilterFunc(IGetFinancialOwners_FinancialOwners element)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        return element.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
               element.Id.ToString().Contains(_searchString);
    }

    private async Task LoadOwners()
    {
        _loading = true;
        try
        {
            var result = await Client.GetFinancialOwners.ExecuteAsync();
            if (result.Data?.FinancialOwners != null)
                _owners = result.Data.FinancialOwners;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao carregar responsáveis: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task OpenCreateDialog()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<FinancialOwnerDialog>("Novo Responsável", new DialogParameters(), options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
            await LoadOwners();
    }

    private async Task OpenEditDialog(IGetFinancialOwners_FinancialOwners owner)
    {
        var parameters = new DialogParameters { ["OwnerId"] = owner.Id, ["OwnerName"] = owner.Name };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<FinancialOwnerDialog>("Editar Responsável", parameters, options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
            await LoadOwners();
    }

    private async Task ConfirmDelete(IGetFinancialOwners_FinancialOwners owner)
    {
        var parameters = new DialogParameters
        {
            ["Title"] = "Confirmar Exclusão",
            ["Message"] = $"Deseja realmente excluir o responsável <b>{owner.Name}</b>?",
            ["ConfirmText"] = "Excluir",
            ["CancelText"] = "Cancelar"
        };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmar Exclusão", parameters);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            try
            {
                var response = await Client.DeleteFinancialOwner.ExecuteAsync(owner.Id);
                if (response.Errors.Any())
                    Snackbar.Add(response.Errors.First().Message, Severity.Warning);
                else
                {
                    Snackbar.Add("Responsável excluído com sucesso!", Severity.Success);
                    await LoadOwners();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Erro ao excluir: {ex.Message}", Severity.Error);
            }
        }
    }
}
