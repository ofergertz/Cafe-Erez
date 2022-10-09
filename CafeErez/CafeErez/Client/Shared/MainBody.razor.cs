using CafeErez.Client.Shared.Extensions;
using CafeErez.Shared.Infrastructure.Localization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static CafeErez.Shared.Constants.Constants;

namespace CafeErez.Client.Shared
{
    public partial class MainBody
    {
        bool _drawerOpen = false;
        bool _rightToLeft = false;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private string FirstName { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var culturePreference = await _localStorageService.GetItemAsync<LanguageCode>(StorageConstants.LocalPreference);
            if (culturePreference == null)
            {
                _rightToLeft = false;
                return;
            }
            _rightToLeft = culturePreference.isRtl;

            if (await UserIsAuthenticated())
                _navigationManager.NavigateTo("/CustomerDiary");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                FirstName = user.GetFirstName();
            }
        }

        private async Task<bool> UserIsAuthenticated()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return false;
            return (user.Identity?.IsAuthenticated == true);
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
