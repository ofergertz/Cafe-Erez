using CafeErez.Shared.Infrastructure.Localization;
using MudBlazor;
using static CafeErez.Shared.Constants.Constants;

namespace CafeErez.Client.Shared
{
    public partial class MainLayout
    {
        private bool _rightToLeft = false;

        protected override async Task OnInitializedAsync()
        {
            var culturePreference = await _localStorageService.GetItemAsync<LanguageCode>(StorageConstants.LocalPreference);
            if (culturePreference == null)
            {
                _rightToLeft = false;
                return;
            }
            _rightToLeft = culturePreference.isRtl;
        }
    }
}
