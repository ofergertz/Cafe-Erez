using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CafeErez.Client.Pages.Authentication
{
    public partial class Register
    {
        private RegisterRequest _registerRequest = new();

        private async Task OnValidSubmit(EditContext context)
        {
            var result = await _authenticationManager.Register(_registerRequest);
            if (result.Succeeded)
            {
                _snackBar.Add($"Registrarion of {_registerRequest.UserName} was succeded", Severity.Success);
                _navigationManager.NavigateTo("/");
            }
            else
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
        }
    }
}
