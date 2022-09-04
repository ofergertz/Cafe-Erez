using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using CafeErez.Shared.Model.Identity;
using MudBlazor;

namespace CafeErez.Client.Shared.Dialogs
{
    public partial class Logout
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string ContentText { get; set; }
        [Parameter] public string ButtonText { get; set; }
        [Parameter] public Color Color { get; set; }

        async Task Submit()
        {
            var result = await _authenticationManager.Logout();
            if (result.Succeeded)
                _navigationManager.NavigateTo("/");
            else
                _snackBar.Add(_localizer["Customer already logged out."], Severity.Info);
        }
        void Cancel() => MudDialog.Cancel();
    }
}
