using CafeErez.Shared.Infrastructure.Localization;
using static CafeErez.Shared.Constants.Constants;

namespace CafeErez.Client.Shared
{
    public partial class MainLayout
    {
        bool _drawerOpen = false;
        bool _rightToLeft = false;

        protected override async Task OnInitializedAsync()
        {
            var culturePreference = await _localStorageService.GetItemAsync<LanguageCode>(StorageConstants.LocalPreference);
            _rightToLeft = culturePreference.isRtl;
        }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }


        private async Task ChangeLanguageAsync(LanguageCode languageCode)
        {
            await _localStorageService.SetItemAsync(StorageConstants.LocalPreference, languageCode);
            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }
    }
}
