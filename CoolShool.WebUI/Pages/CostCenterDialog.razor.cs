using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CoolShool.WebUI.Pages;

public partial class CostCenterDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Inject] private ICoolSchoolClient Client { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public long CenterId { get; set; } = 0;
    [Parameter] public CostCenterType Type { get; set; } = CostCenterType.Mensalidade;
    [Parameter] public string? Name { get; set; }

    private bool _success;
    private bool _processing;
    private MudForm _form = default!;
    private string _title => CenterId == 0 ? "Novo Centro de Custo" : "Editar Centro de Custo";

    private void Cancel() => MudDialog.Cancel();

    private async Task Submit()
    {
        _processing = true;
        try
        {
            if (CenterId == 0)
            {
                var result = await Client.CreateCostCenter.ExecuteAsync(Type, Name);
                if (result.Errors.Any())
                    Snackbar.Add(result.Errors.First().Message, Severity.Error);
                else
                {
                    Snackbar.Add("Centro de custo criado com sucesso!", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(true));
                }
            }
            else
            {
                var result = await Client.UpdateCostCenter.ExecuteAsync(CenterId, Type, Name);
                if (result.Errors.Any())
                    Snackbar.Add(result.Errors.First().Message, Severity.Error);
                else
                {
                    Snackbar.Add("Centro de custo atualizado com sucesso!", Severity.Success);
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
