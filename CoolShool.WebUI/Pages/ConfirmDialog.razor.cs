using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CoolShool.WebUI.Pages;

public partial class ConfirmDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public string Title { get; set; } = "Confirmação";
    [Parameter] public string Message { get; set; } = "Deseja realmente prosseguir?";
    [Parameter] public string ConfirmText { get; set; } = "Excluir";
    [Parameter] public string CancelText { get; set; } = "Cancelar";

    private void Cancel() => MudDialog.Cancel();
    private void Confirm() => MudDialog.Close(DialogResult.Ok(true));
}
