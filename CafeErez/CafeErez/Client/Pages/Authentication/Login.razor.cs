using CafeErez.Shared.BusinessService;
using CafeErez.Shared.Model.Identity;
using MudBlazor;

namespace CafeErez.Client.Pages.Authentication
{
    public partial class Login
    {
        private LoginRequest _loginRequest = new();

        public Login()
        {
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        private async Task Fill()
        {
            _loginRequest.Email = "ofer1g38@gmail.com";
            _loginRequest.Password = "Aa123456!";
        }
        private async Task SubmitAsync()
        {
            var result = await _authenticationManager.Login(_loginRequest);
            if (result.Succeeded)
                _navigationManager.NavigateTo("/CustomerDiary");
            else
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
        }
    }
}
