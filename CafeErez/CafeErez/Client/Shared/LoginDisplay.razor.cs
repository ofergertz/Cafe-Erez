using MudBlazor;

namespace CafeErez.Client.Shared
{
    public partial class LoginDisplay
    {
        private void Logout()
        {
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.Logout.ContentText), $"{_localizer["Logout Confirmation"]}"},
                {nameof(Dialogs.Logout.ButtonText), $"{_localizer["Confirm"]}"},
                {nameof(Dialogs.Logout.Color), Color.Error},
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            _dialogService.Show<Dialogs.Logout>(_localizer["Logout"], parameters, options);
        }
    }
}
