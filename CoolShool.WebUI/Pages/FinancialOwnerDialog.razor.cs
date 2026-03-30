using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CoolShool.WebUI.Pages;

public partial class FinancialOwnerDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Inject] private ICoolSchoolClient Client { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public long OwnerId { get; set; } = 0;
    [Parameter] public string OwnerName { get; set; } = string.Empty;

    private bool _success;
    private bool _processing;
    private MudForm _form = default!;
    private string _title => OwnerId == 0 ? "Cadastro de Responsável" : "Editar Responsável";

    private void Cancel() => MudDialog.Cancel();

    private async Task Submit()
    {
        _processing = true;
        try
        {
            if (OwnerId == 0)
            {
                var result = await Client.CreateFinancialOwner.ExecuteAsync(OwnerName);
                if (result.Errors.Any())
                    Snackbar.Add(result.Errors.First().Message, Severity.Error);
                else
                {
                    Snackbar.Add("Responsável cadastrado com sucesso!", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(true));
                }
            }
            else
            {
                var result = await Client.UpdateFinancialOwner.ExecuteAsync(OwnerId, OwnerName);
                if (result.Errors.Any())
                    Snackbar.Add(result.Errors.First().Message, Severity.Error);
                else
                {
                    Snackbar.Add("Responsável atualizado com sucesso!", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(true));
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao salvar: {ex.Message}", Severity.Error);
        }
        finally
        {
            _processing = false;
        }
    }
}
