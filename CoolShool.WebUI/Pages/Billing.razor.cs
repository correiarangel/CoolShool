using Microsoft.AspNetCore.Components;
using MudBlazor;
using StrawberryShake;

namespace CoolShool.WebUI.Pages;

public partial class Billing : ComponentBase
{
    [Inject] public ICoolSchoolClient Client { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private bool _isLoading = true;
    private bool _isProcessing = false;
    private string? _searchString;
    private bool _drawerOpen;
    private IGetBillings_Billings? _selectedBilling;
    private List<IGetBillings_Billings> _billings = new();

    // Campos de formulário no Drawer
    private DateTime? _paymentDate = DateTime.Today;
    private decimal _paymentAmount;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        _isLoading = true;
        try
        {
            var result = await Client.GetBillings.ExecuteAsync();
            if (result.Data != null)
            {
                _billings = result.Data.Billings.ToList();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao carregar dados: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void OpenDetails(IGetBillings_Billings billing)
    {
        _selectedBilling = billing;
        _paymentAmount = billing.Amount;
        _paymentDate = DateTime.Today;
        _drawerOpen = true;
    }

    private async Task RegisterPayment()
    {
        if (_selectedBilling == null) return;
        
        _isProcessing = true;
        try
        {
            var result = await Client.RegisterPayment.ExecuteAsync(
                _selectedBilling.Id, 
                _paymentAmount, 
                _paymentDate ?? DateTime.UtcNow);

            if (result.IsSuccessResult())
            {
                Snackbar.Add("Pagamento registrado com sucesso!", Severity.Success);
                _drawerOpen = false;
                await LoadData();
            }
            else
            {
                Snackbar.Add(string.Join(", ", result.Errors.Select(e => e.Message)), Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private async Task ConfirmCancelCharge()
    {
        if (_selectedBilling == null) return;

        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.Title, "Atenção: Cancelamento" },
            { x => x.Message, $"Deseja realmente cancelar a cobrança #{_selectedBilling.Id} no valor de {_selectedBilling.Amount:C}?" },
            { x => x.ConfirmText, "Cancelar Cobrança" }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancelar Cobrança", parameters, options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            await CancelCharge();
        }
    }

    private async Task CancelCharge()
    {
        if (_selectedBilling == null) return;

        _isProcessing = true;
        try
        {
            var result = await Client.CancelCharge.ExecuteAsync(_selectedBilling.Id);
            if (result.IsSuccessResult())
            {
                Snackbar.Add("Cobrança cancelada com sucesso!", Severity.Success);
                _drawerOpen = false;
                await LoadData();
            }
            else
            {
                Snackbar.Add(string.Join(", ", result.Errors.Select(e => e.Message)), Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private bool FilterFunc(IGetBillings_Billings element)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;
        
        return element.Id.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
               $"{element.Amount}".Contains(_searchString);
    }
}
