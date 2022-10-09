using CafeErez.Shared.Infrastructure.Localization;
using MudBlazor;
using static CafeErez.Shared.Constants.Constants;

namespace CafeErez.Client.Shared
{
    public partial class LoginDisplay
    {
        private async Task ChangeLanguageAsync(LanguageCode languageCode)
        {
            await _localStorageService.SetItemAsync(StorageConstants.LocalPreference, languageCode);
            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }
    }
}
