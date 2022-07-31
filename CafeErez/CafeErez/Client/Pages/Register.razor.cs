using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Identity;
using Microsoft.AspNetCore.Components.Forms;

namespace CafeErez.Client.Pages
{
    public partial class Register
    {
        bool success;
        private RegisterRequest _registerRequest = new();

        private void OnValidSubmit(EditContext context)
        {
            success = true;
            StateHasChanged();
        }
    }
}
